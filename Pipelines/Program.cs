// See https://aka.ms/new-console-template for more information
/*
 1. 调用 PipeWriter.GetMemory(Int32) 从基础编写器获取内存。
 2. 调用 PipeWriter.Advance(Int32) 以告知 PipeWriter 有多少数据已写入缓冲区。
 3. 调用 PipeWriter.FlushAsync 以使数据可用于 PipeReader。
 */
using System.Buffers;
using System.IO.Pipelines;
using System.Net.Sockets;

Console.WriteLine("Hello, World!");

async Task ProcessLinesAsync(Socket socket)
{
    var pipe = new Pipe();
    Task writing = FillPipeAsync(socket, pipe.Writer);
    Task reading = ReadPipeAsync(pipe.Reader);

    await Task.WhenAll(writing, reading);
}

async Task FillPipeAsync(Socket socket, PipeWriter writer)
{
    const int miniumBufferSize = 512;
    while (true)
    {
        // 从 PipeWriter 分配至少 512 字节内存
        Memory<byte> memory = writer.GetMemory(miniumBufferSize);
        try
        {
            int bytesRead = await socket.ReceiveAsync(memory, SocketFlags.None);
            if (bytesRead == 0)
            {
                break;
            }
            // 告诉 PipeWriter 已经从 socket 读了多少数据
            writer.Advance(bytesRead);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            break;
        }

        // 确保 PipeReader 数据可用
        FlushResult result = await writer.FlushAsync();
        if (result.IsCompleted)
        {
            break;
        }
    }
    // 标记已完成，告诉 pipereader 不再有数据进来
    await writer.CompleteAsync();
}

async Task ReadPipeAsync(PipeReader reader)
{
    while (true)
    {
        ReadResult result = await reader.ReadAsync();
        ReadOnlySequence<byte> buffer = result.Buffer;

        while (TryReadLine(ref buffer, out ReadOnlySequence<byte> line))
        {
            // 处理每行
            ProcessLine(line);
        }

        // 告诉 PipeReader 已经消费了多少缓冲区
        reader.AdvanceTo(buffer.Start, buffer.End);
        if (result.IsCompleted)
        {
            break;
        }
    }
    await reader.CompleteAsync();
}

bool TryReadLine(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> line)
{
    // 换行符 EOF 标识
    SequencePosition? position = buffer.PositionOf((byte)'\n');
    if (position == null)
    {
        line = default;
        return false;
    }

    line = buffer.Slice(0, position.Value);
    // 跳过换行符
    buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
    return true;
}

void PipeOptionsDemo()
{
    // Pipe 将开始从 FlushAsync 返回未完成的任务，直到读取器检查至少 5 个字节。
    var options = new PipeOptions(pauseWriterThreshold:10,resumeWriterThreshold:5);
    var pipe = new Pipe(options);
}
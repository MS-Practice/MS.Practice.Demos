using Lib.AspNetCore.ServerSentEvents;
using Microsoft.AspNetCore.ResponseCompression;
using ServerSendEvent.HostServices;
using System.Buffers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddServerSentEvents();
builder.Services.AddResponseCompression(options => {
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "text/event-stream" });
});
//builder.Services.AddHostedService<HeartbeatService>();
builder.Services.AddHostedService<ServerEventsWorker>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.UseResponseCompression()
    .UseStaticFiles()
    .UseRouting()
    .UseEndpoints(endpoint => {
        endpoint.MapControllers();
        //endpoint.MapServerSentEvents("/see-heartbeat");
        endpoint.MapServerSentEvents("/chat-completion");
        endpoint.Map("/origin-sse", async (context) =>
        {
            for (int i = 0; i < 10; i++)
            {
                string txt = $"Demo.AspNetCore.ServerSentEvents Heartbeat ({DateTime.UtcNow} UTC)";
                context.Response.ContentType = "text/event-stream";
                string dataField = $"data:{txt}\n\n"; // \n\n 表示结束
                await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(dataField));
                await context.Response.Body.FlushAsync();
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        });
        endpoint.Map("/origin-sse2", async (context) =>
        {
            for (int i = 0; i < 10; i++)
            {
                context.Response.ContentType = "text/event-stream";
                string dataField = $"""
data:Demo.AspNetCore.ServerSentEvents Heartbeat ({DateTime.UtcNow} UTC)
id:{RandomNumberGenerator.GetInt32(10000,99999)}
type:message


""";
                // 上述格式不好看，可以修正以下：
                string dataField2 = $"""
data:Demo.AspNetCore.ServerSentEvents Heartbeat ({DateTime.UtcNow} UTC)
id:{RandomNumberGenerator.GetInt32(10000, 99999)}
type:message
""";
                dataField2 += "\n\n";
                await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(dataField));
                await context.Response.Body.FlushAsync();
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        });
    })
    ;

app.Run();

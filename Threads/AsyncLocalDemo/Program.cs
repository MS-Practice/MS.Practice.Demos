// See https://aka.ms/new-console-template for more information
using AsyncLocalDemo;

_asyncLocal.Value = "A";

Console.WriteLine($"AsyncLocal before FooAsync: {_asyncLocal.Value}");

await FooAsync();

Console.WriteLine($"AsyncLocal after await FooAsync: {_asyncLocal.Value}");

_threadLocal.Value = "a";
Console.WriteLine($"AsyncLocal before BarAsync: {_threadLocal.Value}");
await BarAsync();
Console.WriteLine($"AsyncLocal before BarAsync: {_threadLocal.Value}");

_valueAccessor.Value = "1";
Console.WriteLine($"ValueAccessor before await FooBarAsync in Main: {_valueAccessor.Value}");

await FooBarAsync();

Console.WriteLine($"ValueAccessor after await FooBarAsync in Main: {_valueAccessor.Value}");



public partial class Program
{
    private static AsyncLocal<string> _asyncLocal = new AsyncLocal<string>();

    private static async Task FooAsync()
    {
        _asyncLocal.Value = "B";

        Console.WriteLine($"AsyncLocal before await in FooAsync: {_asyncLocal.Value}");

        await Task.Delay(100);

        Console.WriteLine($"AsyncLocal after await in FooAsync: {_asyncLocal.Value}");
    }

    private static ThreadLocal<string> _threadLocal = new ThreadLocal<string>();

    private static async Task BarAsync()
    {
        _threadLocal.Value = "b";

        Console.WriteLine($"AsyncLocal before await in BarAsync: {_threadLocal.Value}");

        await Task.Delay(100);

        Console.WriteLine($"AsyncLocal after await in BarAsync: {_threadLocal.Value}");
    }

    private static IValueAccessor<string> _valueAccessor = new ValueAccessor<string>();
    private static async Task FooBarAsync()

    {
        _valueAccessor.Value = "2";

        Console.WriteLine($"ValueAccessor before await in FooBarAsync: {_valueAccessor.Value}");

        await Task.Delay(100);

        Console.WriteLine($"ValueAccessor after await in FooBarAsync: {_valueAccessor.Value}");

    }
}



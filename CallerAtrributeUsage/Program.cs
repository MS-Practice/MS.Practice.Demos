// See https://aka.ms/new-console-template for more information
using System.Runtime.CompilerServices;

Console.WriteLine("Hello, World!");
Bar b = null;
try
{
    CallerAttributeUsage.ArgumentNotNull(b);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}


Invoker.InvokeInvoker();
Console.ReadLine();

public static class CallerAttributeUsage
{
    public static T ArgumentNotNull<T>(T argumentValue, [CallerArgumentExpression("argumentValue")] string argumentName = "")
        where T : class
    {
        if (argumentValue is null)
            throw new ArgumentNullException(argumentName);

        return argumentValue;
    }
    // CallerMemberName 返回 caller 的入口方法名称
    public static void Invoke([CallerMemberName] string name = "") => Console.WriteLine($"Invoke by {name}");
    public static void InvokeWithContext([CallerMemberName] string name = "", [CallerFilePath]string? filePath = default, [CallerLineNumber]int lineNumber = default) => Console.WriteLine($"Invoke by {name}\r\nCallerFilePath:{filePath}\r\nCallerLineNumber:{lineNumber}");
}

public class Bar
{
    public static void InvokeBar()
    {
        CallerAttributeUsage.Invoke();
        CallerAttributeUsage.InvokeWithContext();
    }
}

public class Invoker
{
    public static void InvokeInvoker()
    {
        CallerAttributeUsage.Invoke();
        CallerAttributeUsage.InvokeWithContext();
        Bar.InvokeBar();
    }
}



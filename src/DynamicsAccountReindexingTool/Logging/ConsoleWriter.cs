using Microsoft.Extensions.Logging;

public class ConsoleWriter : ILogger
{
    private static object _MessageLock = new object();

    public void SetMessageFormat(LogLevel logLevel)
    {
        switch (logLevel)
        {
            case LogLevel.Information:
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkGray;
                break;
            case LogLevel.Critical:
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                break;
            case LogLevel.Warning:
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                break;
            case LogLevel.Error:
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkRed;
                break;
            case LogLevel.None:
            default:
                Console.ResetColor();
                break;
        }
    }

    public void WriteMessage(string message, LogLevel logLevel)
    {
        lock (_MessageLock)
        {
            SetMessageFormat(logLevel);
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }
        WriteMessage(state?.ToString() ?? string.Empty, logLevel);
    }
}
public class ConsoleWriter : IDynamicsToolLogger
{
    private static object _MessageLock = new object();

    public void SetMessageFormat(LoggerFormatOptions formatOptions)
    {
        switch (formatOptions)
        {
            case LoggerFormatOptions.Info:
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkGray;
                break;
            case LoggerFormatOptions.Prompt:
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                break;
            case LoggerFormatOptions.Warning:
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                break;
            case LoggerFormatOptions.Error:
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkRed;
                break;
            case LoggerFormatOptions.Custom:
            case LoggerFormatOptions.None:
            default:
                Console.ResetColor();
                break;
        }
    }

    public void WriteMessage(string message, LoggerFormatOptions formatOptions)
    {
        lock (_MessageLock)
        {
            SetMessageFormat(formatOptions);
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
public static class ConsoleWriter
{
    private static object _MessageLock = new object();

    public static void WriteMessage(string message)
    {
        Console.ResetColor();
        WriteMessage(message, Console.ForegroundColor, Console.BackgroundColor);
    }

    public static void WriteInfo(string message)
    {
        WriteMessage(message, ConsoleColor.Gray, ConsoleColor.DarkGray);
    }

    public static void WritePrompt(string message)
    {
        WriteMessage(message, ConsoleColor.Cyan, ConsoleColor.DarkCyan);
    }

    public static void WriteWarning(string message)
    {
        WriteMessage(message, ConsoleColor.Yellow, ConsoleColor.DarkYellow);
    }

    public static void WriteError(string message)
    {
        WriteMessage(message, ConsoleColor.Red, ConsoleColor.DarkRed);
    }

    public static void WriteMessage(string message, ConsoleColor foreground, ConsoleColor background)
    {
        lock (_MessageLock)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }

    public static void SetMessageColor(ConsoleColor foreground, ConsoleColor background)
    {
        lock (_MessageLock)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
        }
    }

     public static void ResetMessageColor()
    {
        lock (_MessageLock)
        {
             Console.ResetColor();
        }
    }


}
using Microsoft.Crm.Sdk.Messages;

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
        WriteMessage(message, ConsoleColor.White, ConsoleColor.DarkGray);
    }

    public static void WritePrompt(string message)
    {
        WriteMessage(message, ConsoleColor.White, ConsoleColor.DarkCyan);
    }

    public static void WriteWarning(string message)
    {
        WriteMessage(message, ConsoleColor.White, ConsoleColor.DarkYellow);
    }

    public static void WriteError(string message)
    {
        WriteMessage(message, ConsoleColor.White, ConsoleColor.DarkRed);
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

    public static bool GiveConfirmPrompt()
    {
        bool confirmed;
        ConsoleKey response;
        ConsoleWriter.WritePrompt("Please confirm [y/n] ");
        var isValidInput = false;
        do
        {
            response = Console.ReadKey(false).Key;
            if (response != ConsoleKey.Enter)
                Console.WriteLine();
            isValidInput = response == ConsoleKey.Y || response == ConsoleKey.N;
            if (!isValidInput)
            {
                ConsoleWriter.WriteWarning("No valid entry, please try again!");
            }
            confirmed = response == ConsoleKey.Y;
        } while (!isValidInput);
        return confirmed;
    }

    public static string ReadKey(bool intercept = true)
    {
        var input = Console.ReadKey(true);
        return input.KeyChar.ToString();
    }

    public static string? ReadLine()
    {
       return Console.ReadLine();
    }
}
public class ConsoleReader : IDynamicsToolInput
{
    public ConsoleReader(IDynamicsToolLogger logger) {
        _logger = logger;
    }

    private IDynamicsToolLogger _logger;

    public char GetCharacterInput()
    {
        var input = Console.ReadKey(true);
        return input.KeyChar;
    }

    public bool GetConfirmationInput()
    {
        bool confirmed;
        ConsoleKey response;
        _logger.WriteMessage("Please confirm [y/n] ", LoggerFormatOptions.Prompt);
        var isValidInput = false;
        do
        {
            response = Console.ReadKey(false).Key;
            if (response != ConsoleKey.Enter)
                Console.WriteLine();
            isValidInput = response == ConsoleKey.Y || response == ConsoleKey.N;
            if (!isValidInput)
            {
                _logger.WriteMessage("No valid entry, please try again!", LoggerFormatOptions.Warning);
            }
            confirmed = response == ConsoleKey.Y;
        } while (!isValidInput);
        return confirmed;
    }

    public string GetStringInput()
    {
        return Console.ReadLine() ?? String.Empty;
    }
}
using Microsoft.Extensions.Logging;

public class DynamicsNoninteractiveManager : IDynamicsToolInput
{
    public DynamicsNoninteractiveManager(ILogger logger)
    {
        _logger = logger;
    }

    private ILogger _logger;

    private void ValidateInteractiveMode<T>(T? defaultValue)
    {
        if (defaultValue != null)
        {
            return;
        }
        var errorMessage = "Interactive mode is active but no default input was provided!";
        _logger.LogError(errorMessage);
        throw new Exception(errorMessage);
    }

    public void SetInteractiveMode(bool isInterActive)
    {
    }

    public bool GetConfirmationInput(bool? defaultValue = null)
    {
        ValidateInteractiveMode(defaultValue);
        return defaultValue!.Value;
    }

    public char GetCharacterInput(char? defaultValue = null)
    {
        ValidateInteractiveMode(defaultValue);
        return defaultValue!.Value;
    }

    public string GetStringInput(string? defaultValue = null)
    {
        ValidateInteractiveMode(defaultValue);
        return defaultValue!;
    }
}
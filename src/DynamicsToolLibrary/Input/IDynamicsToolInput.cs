public interface IDynamicsToolInput
{
    void SetInteractiveMode(bool isInterActive);
    bool GetConfirmationInput(bool? defaultValue = null);
    char GetCharacterInput(char? defaultValue = null);
    string GetStringInput(string? defaultValue = null);
}
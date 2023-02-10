public interface IDynamicsToolLogger
{
    void SetMessageFormat(LoggerFormatOptions formatOptions);
    void WriteMessage(string message, LoggerFormatOptions formatOptions);
}
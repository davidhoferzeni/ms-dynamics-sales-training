using Microsoft.Extensions.Logging;

public interface IDynamicsToolLogger : ILogger
{
    void SetMessageFormat(LogLevel logLevel);
}
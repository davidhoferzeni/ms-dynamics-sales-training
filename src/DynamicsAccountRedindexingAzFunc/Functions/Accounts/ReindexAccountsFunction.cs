using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class DynamicsAccountReindexFunction
{
    private readonly ILogger _logger;

    public DynamicsAccountReindexFunction(ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _logger = loggerFactory.CreateLogger<DynamicsAccountReindexFunction>();
    }

    [Function("ReindexAccountsFunction")]
    public void Run([TimerTrigger("0 * * * * *", 
#if DEBUG
    RunOnStartup = true
#endif
    )] TimerInfo timerInfo
    )
    {
        timerInfo.LogStatus(_logger);
        
    }
}
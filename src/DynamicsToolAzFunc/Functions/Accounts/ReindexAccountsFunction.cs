using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class DynamicsAccountReindexFunction
{
    private readonly ILogger _logger;
    private readonly DynamicsRoutines _dynamicsRoutines;

    public DynamicsAccountReindexFunction(ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _logger = loggerFactory.CreateLogger<DynamicsAccountReindexFunction>();
        _dynamicsRoutines = new DynamicsRoutines(configuration, _logger);
    }

    [Function("ReindexAccountsFunction")]
    public void Run([TimerTrigger("0 0 0 * * *" 
#if DEBUG
    , RunOnStartup = true
#endif
    )] TimerInfo timerInfo
    )
    {
        timerInfo.LogStatus(_logger);
        _dynamicsRoutines.StartAccountReindexRoutine();
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();
var logger = new ConsoleWriter();
var inputManager = new ConsoleReader(logger);
logger.LogInformation("Welcome to the Dynamics Account Reindexing Tool!");
logger.LogInformation("This tool will present all current accounts of your Dynamics Sales platform to you and reindex them in alphabetical order.");

var dynamiceRoutineLogic = new DynamicsRoutines(config, logger, inputManager);
dynamiceRoutineLogic.StartAccountReindexRoutine();

// Pause the console so it does not close.
logger.LogCritical("Press any key to exit.");
inputManager.GetCharacterInput('n');
using Microsoft.Extensions.Configuration;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();
var logger = new ConsoleWriter();
var inputManager = new ConsoleReader(logger);

logger.WriteMessage("Welcome to the Dynamics Account Reindexing Tool!", LoggerFormatOptions.Info);
logger.WriteMessage("This tool will present all current accounts of your Dynamics Sales platform to you and reindex them in alphabetical order.", LoggerFormatOptions.Info);

var dynamiceRoutineLogic = new DynamicsRoutines(config, logger, inputManager);
dynamiceRoutineLogic.StartAccountReindexRoutine();

// Pause the console so it does not close.
logger.WriteMessage("Press any key to exit.", LoggerFormatOptions.Prompt);
inputManager.GetCharacterInput('n');
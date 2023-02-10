using Microsoft.Extensions.Configuration;

ConsoleWriter.WriteInfo("Welcome to the Dynamics Account Reindexing Tool!");
ConsoleWriter.WriteInfo("This tool will present all current accounts of your Dynamics Sales platform to you and reindex them in alphabetical order.");

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

var dynamiceRoutineLogic = new DynamicsRoutines(config);
dynamiceRoutineLogic.StartAccountReindexRoutine();

// Pause the console so it does not close.
ConsoleWriter.WritePrompt("Press any key to exit.");
ConsoleWriter.ReadKey();
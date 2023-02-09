using Microsoft.Extensions.Configuration;
using ConsoleTables;

// See https://aka.ms/new-console-template for more information
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

var dynamiceRoutineLogic = new DynamicsRoutines(config);
dynamiceRoutineLogic.StartAccountReindexRoutine();

// Pause the console so it does not close.
ConsoleWriter.WritePrompt("Press any key to exit.");
Console.ReadKey(true);
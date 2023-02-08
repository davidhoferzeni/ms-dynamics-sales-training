using Microsoft.Extensions.Configuration;
using ConsoleTables;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

var connectionSettings = ConfigurationSectionBuilder<ConnectionConfiguration>.GetConfigurationSection(config, "ConnectionConfiguration");

if (connectionSettings == null)
{
    throw new Exception("Settings are not available!");
}

Console.WriteLine($"Trying to connect to {connectionSettings.ConnectionString}");

var dynamicsSession = new DynamicsSession(connectionSettings);
var accountLogic = new AccountLogic(dynamicsSession);
var accountEntities = accountLogic.GetAccountEntities();
AccountLogic.Reindex(accountEntities);
ConsoleTable.From<AccountEntity>(accountEntities).Write();

// Pause the console so it does not close.
Console.WriteLine("Press any key to exit.");
Console.ReadKey(true);
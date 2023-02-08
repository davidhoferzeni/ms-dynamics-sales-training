using Microsoft.Extensions.Configuration;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using ConsoleTables;
using Microsoft.PowerPlatform.Dataverse.Client.Extensions;


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

using (ServiceClient serviceClient = new(connectionSettings.ConnectionString))
if (serviceClient.IsReady)
{
    QueryExpression query = new QueryExpression()
    {
        EntityName = "account",
        ColumnSet = new ColumnSet("name", "new_accountindex")
    };

    EntityCollection accountCollection = serviceClient.RetrieveMultiple(query);
    var accountEntities = accountCollection.Entities.Select(a => DynamicsEntityBuilder<AccountEntity>.Build(a)).OrderBy(a => a?.AccountName).ToList();

    foreach (var (account, index) in accountEntities.Select((a, i) => (a, i)))
    {
        if (account == null)
        {
            continue;
        }
        account.NewAccountIndex = (uint?)index;
    }
    ConsoleTable.From<AccountEntity>(accountEntities).Write();
}
else
{
    Console.WriteLine(
        "A web service connection was not established.");
}


// Pause the console so it does not close.
Console.WriteLine("Press any key to exit.");
Console.ReadKey(true);
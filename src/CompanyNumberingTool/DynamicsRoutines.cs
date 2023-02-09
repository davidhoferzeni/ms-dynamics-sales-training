
using System.Threading.Tasks.Dataflow;
using ConsoleTables;
using Microsoft.Extensions.Configuration;
using Microsoft.PowerPlatform.Dataverse.Client;

public class DynamicsRoutines
{
    private ConnectionConfiguration _connectionConfiguration;
    private StartupConfiguration _startupConfiguration;

    private DynamicsSession? _session;

    private DynamicsSession Session {
       get
        {
            if (_session == null)
            {
                _session = new DynamicsSession(_connectionConfiguration);
            }
            return _session;
        }
    }

    public DynamicsRoutines(IConfiguration config)
    {
       _connectionConfiguration = ConfigurationSectionBuilder<ConnectionConfiguration>.GetConfigurationSection(config, "ConnectionConfiguration");
       _startupConfiguration= ConfigurationSectionBuilder<StartupConfiguration>.GetConfigurationSection(config, "StartupConfiguration");
    }

    public void StartAccountReindexRoutine() {
        var accountLogic = new AccountLogic(Session);
        ConsoleWriter.WriteInfo("Fetching accounts from database.");
        var accountEntities = accountLogic.GetAccountEntities();
        AccountLogic.Reindex(accountEntities, _startupConfiguration.InitialCompanyIndex ?? 1);
        ConsoleWriter.SetMessageColor(ConsoleColor.White, ConsoleColor.DarkGray);
        ConsoleTable.From<AccountEntity>(accountEntities).Write();
        ConsoleWriter.ResetMessageColor();
        ConsoleWriter.WriteInfo("Do you want to update the index number for the presented accounts?");
        var confirmTransaction = ConsoleWriter.GiveConfirmPrompt();
        if (confirmTransaction) {
            ConsoleWriter.WriteInfo("Updating accounts with new index number.");
            accountLogic.UpdateAccountEntities(accountEntities);
            ConsoleWriter.WriteInfo($"Successfully updated {accountEntities.Count} accounts.");
        } else {
            ConsoleWriter.WriteError("Transaction was cancelled by user.");
        }
    }
}

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
        var accountEntities = accountLogic.GetAccountEntities();
        AccountLogic.Reindex(accountEntities, _startupConfiguration.InitialCompanyIndex ?? 1);
        ConsoleWriter.SetMessageColor(ConsoleColor.Magenta, ConsoleColor.DarkMagenta);
        ConsoleTable.From<AccountEntity>(accountEntities).Write();
        ConsoleWriter.ResetMessageColor();
        accountLogic.UpdateAccountEntities(accountEntities);
    }
}
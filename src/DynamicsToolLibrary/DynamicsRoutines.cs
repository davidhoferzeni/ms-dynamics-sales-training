
using System.Threading.Tasks.Dataflow;
using ConsoleTables;
using Microsoft.Extensions.Configuration;
using Microsoft.PowerPlatform.Dataverse.Client;

public class DynamicsRoutines
{
    private ConnectionConfiguration _connectionConfiguration;
    private StartupConfiguration _startupConfiguration;
    private IDynamicsToolInput _inputManager;
    private IDynamicsToolLogger _logger;

    private DynamicsSession? _session;

    private DynamicsSession Session
    {
        get
        {
            if (_session == null)
            {
                _session = new DynamicsSession(_connectionConfiguration, _logger);
            }
            return _session;
        }
    }

    public DynamicsRoutines(IConfiguration config, IDynamicsToolLogger logger, IDynamicsToolInput inputManager)
    {
        var nonInteractiveMode = config.GetValue<bool?>("NonInteractive");
        _logger = logger;
        _inputManager = inputManager;
        if (nonInteractiveMode.HasValue) {
            _inputManager.SetInteractiveMode(!nonInteractiveMode.Value);
        }
        _startupConfiguration = ConfigurationSectionBuilder<StartupConfiguration>.GetConfigurationSection(config, logger, inputManager, "StartupConfiguration");
        _connectionConfiguration = ConfigurationSectionBuilder<ConnectionConfiguration>.GetConfigurationSection(config, logger, inputManager, "ConnectionConfiguration");
    }

    public void StartAccountReindexRoutine()
    {
        var accountLogic = new AccountLogic(Session);
        _logger.WriteMessage("Fetching accounts from database.", LoggerFormatOptions.None);
        var accountEntities = accountLogic.GetAccountEntities();
        AccountLogic.Reindex(accountEntities, _startupConfiguration.InitialAccountIndex ?? 1);
        _logger.SetMessageFormat(LoggerFormatOptions.Info);
        ConsoleTable.From<AccountEntity>(accountEntities).Write();
        _logger.WriteMessage("Do you want to update the index number for the presented accounts?", LoggerFormatOptions.Prompt);
        var confirmTransaction = _inputManager.GetConfirmationInput(true);
        if (confirmTransaction)
        {
            _logger.WriteMessage("Updating accounts with new index number.", LoggerFormatOptions.Info);
            accountLogic.UpdateAccountEntities(accountEntities);
            _logger.WriteMessage($"Successfully updated {accountEntities.Count} accounts.", LoggerFormatOptions.Info);
        }
        else
        {
            _logger.WriteMessage("Transaction was cancelled by user.", LoggerFormatOptions.Error);
        }
    }
}
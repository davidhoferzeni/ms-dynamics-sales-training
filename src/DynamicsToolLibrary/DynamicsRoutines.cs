
using System.Threading.Tasks.Dataflow;
using ConsoleTables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;

public class DynamicsRoutines
{
    private ConnectionConfiguration _connectionConfiguration;
    private StartupConfiguration _startupConfiguration;
    private IDynamicsToolInput _inputManager;
    private ILogger _logger;

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

    public DynamicsRoutines(IConfiguration config, ILogger logger, IDynamicsToolInput inputManager)
    {
        var nonInteractiveMode = config.GetValue<bool?>("NonInteractive");
        _logger = logger;
        _inputManager = inputManager;
        if (nonInteractiveMode.HasValue)
        {
            _inputManager.SetInteractiveMode(!nonInteractiveMode.Value);
        }
        _connectionConfiguration = ConfigurationSectionBuilder<ConnectionConfiguration>.GetConfigurationSection(config, logger, inputManager, "ConnectionConfiguration");
        _startupConfiguration = ConfigurationSectionBuilder<StartupConfiguration>.GetConfigurationSection(config, logger, inputManager, "StartupConfiguration");
    }

    public void StartAccountReindexRoutine()
    {
        var accountLogic = new AccountLogic(Session);
        _logger.Log(LogLevel.None, "Fetching accounts from database.");
        var accountEntities = accountLogic.GetList();
        if (accountEntities == null)
        {
            _logger.LogError("No accounts found!");
            return;
        }
        AccountLogic.Reindex(accountEntities, _startupConfiguration.InitialAccountIndex ?? 1);
        var tableOutput = ConsoleTable.From<AccountEntity>(accountEntities).ToString();
        _logger.LogInformation(tableOutput);
        _logger.LogInformation("Do you want to update the index number for the presented accounts?");
        var confirmTransaction = _inputManager.GetConfirmationInput(true);
        if (confirmTransaction)
        {
            _logger.LogInformation("Updating accounts with new index number.");
            accountLogic.Update(accountEntities);
            _logger.LogInformation($"Successfully updated {accountEntities.Count} accounts.");
        }
        else
        {
            _logger.LogError("Transaction was cancelled by user.");
        }
    }
}
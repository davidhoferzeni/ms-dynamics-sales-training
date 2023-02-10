

using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Query;

public class DynamicsSession
{
    public DynamicsSession(ConnectionConfiguration configuration, IDynamicsToolLogger logger, bool autologin = false)
    {
        _configuration = configuration;
        _logger = logger;
    }
    private ConnectionConfiguration _configuration;
    private IDynamicsToolLogger _logger;

    private ServiceClient? _serviceClient;

    public ServiceClient? ServiceClient
    {
        get
        {
            if (_serviceClient == null)
            {
                Login();
            }
            return _serviceClient;
        }
    }

    public void Login()
    {
        _logger.WriteMessage($"Trying to connect to {_configuration.ConnectionString}", LoggerFormatOptions.None);
        _serviceClient = new(_configuration.ConnectionString);
        if (!_serviceClient.IsReady)
        {
            var errorMessage = $"Login to {_configuration.ConnectionString} failed!";
            _logger.WriteMessage(errorMessage, LoggerFormatOptions.Error);
            throw new Exception(errorMessage);
        }
    }
}
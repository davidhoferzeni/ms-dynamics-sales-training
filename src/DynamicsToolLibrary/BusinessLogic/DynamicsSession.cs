
using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Query;

public class DynamicsSession
{
    public DynamicsSession(ConnectionConfiguration configuration, ILogger logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    private ConnectionConfiguration _configuration;
    private ILogger _logger;

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
        _logger.Log(LogLevel.None, $"Trying to connect to {_configuration.ConnectionString}");
        _serviceClient = new(_configuration.ConnectionString);
        if (!_serviceClient.IsReady)
        {
            var errorMessage = $"Login to {_configuration.ConnectionString} failed!";
            _logger.LogError(errorMessage);
            throw new Exception(errorMessage);
        }
    }
}
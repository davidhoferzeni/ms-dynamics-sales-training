

using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Query;

public class DynamicsSession
{
    public DynamicsSession(ConnectionConfiguration configuration, bool autologin = false)
    {
        _configuration = configuration;
    }
    private ConnectionConfiguration _configuration;

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
        Console.WriteLine($"Trying to connect to {_configuration.ConnectionString}");
        _serviceClient = new(_configuration.ConnectionString);
        if (!_serviceClient.IsReady)
        {
            throw new Exception($"Login to {_configuration.ConnectionString} failed!");
        }
    }
}
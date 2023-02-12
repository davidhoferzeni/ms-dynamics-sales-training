using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace Company.Function
{
public class DynamicsAccountCrudFunction
{
    private readonly ILogger _logger;
    private readonly DynamicsCrudLogic<AccountEntity> _accountLogic;

    public DynamicsAccountCrudFunction(ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _logger = loggerFactory.CreateLogger<DynamicsAccountCrudFunction>();
         var connectionConfiguration = ConfigurationSectionBuilder<ConnectionConfiguration>.GetConfigurationSection(configuration, _logger, new DynamicsNoninteractiveManager(_logger), "ConnectionConfiguration");
        var session = new DynamicsSession(connectionConfiguration, _logger);
        _accountLogic = new DynamicsCrudLogic<AccountEntity>(session);
    }

    [Function("DynamicsAccountCrudFunction")]
    public ObjectResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        
        var accountList = _accountLogic.GetList();
        return new OkObjectResult(accountList);
    }
}

}

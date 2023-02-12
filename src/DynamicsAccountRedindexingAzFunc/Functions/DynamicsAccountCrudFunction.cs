using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


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
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "accounts/")] HttpRequestData req)
    {
        _logger.LogInformation("message logged");
        var accountList = _accountLogic.GetList();
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json");
        response.WriteString(JsonSerializer.Serialize(accountList));
        return response;
    }
}

using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


public class GetAccountByNumberFunction
{
    private readonly ILogger _logger;
    private readonly DynamicsCrudLogic<AccountEntity> _accountLogic;

    public GetAccountByNumberFunction(ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _logger = loggerFactory.CreateLogger<GetAccountByNumberFunction>();
        _accountLogic = AzureFunctionHelper.GetEntityLogic<AccountEntity>(_logger, configuration);
    }

    [Function("GetAccountByNumberFunction")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "companies/{number}")] HttpRequestData req, string number)
    {
        var account = _accountLogic.GetByProperty("new_accountindex", number)?.FirstOrDefault();
        var returnCode = account != null? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return AzureFunctionHelper.GetHttpResponseObject(req, returnCode, account);
    }
}

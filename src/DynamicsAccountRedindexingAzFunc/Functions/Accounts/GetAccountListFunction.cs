using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


public class GetAccountListFunction
{
    private readonly ILogger _logger;
    private readonly DynamicsCrudLogic<AccountEntity> _accountLogic;

    public GetAccountListFunction(ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _logger = loggerFactory.CreateLogger<GetAccountListFunction>();
        _accountLogic = AzureFunctionHelper.GetEntityLogic<AccountEntity>(_logger, configuration);
    }

    [Function("GetAccountListFunction")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "companies")] HttpRequestData req)
    {
        var accountList = _accountLogic.GetList();
        return AzureFunctionHelper.GetHttpResponseObject(req, HttpStatusCode.OK, accountList);
    }
}

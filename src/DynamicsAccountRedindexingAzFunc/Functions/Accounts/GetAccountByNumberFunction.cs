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
    private readonly DynamicsRoutines _dynamicsRoutines;

    public GetAccountByNumberFunction(ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _logger = loggerFactory.CreateLogger<GetAccountByNumberFunction>();
        _dynamicsRoutines = new DynamicsRoutines(configuration, _logger);
    }

    [Function("GetAccountByNumberFunction")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "companies/{number}")] HttpRequestData req, string number)
    {
        var accountLogic = _dynamicsRoutines.GetAccountLogic();
        var account = accountLogic.GetByProperty("new_accountindex", number)?.FirstOrDefault();
        var returnCode = account != null? HttpStatusCode.OK : HttpStatusCode.NotFound;
        return AzureFunctionHelper.GetHttpResponseObject(req, returnCode, account);
    }
}

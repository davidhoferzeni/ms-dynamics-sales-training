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
    private readonly DynamicsRoutines _dynamicsRoutines;

    public GetAccountListFunction(ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _logger = loggerFactory.CreateLogger<GetAccountListFunction>();
        _dynamicsRoutines = new DynamicsRoutines(configuration, _logger);
    }

    [Function("GetAccountListFunction")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "companies")] HttpRequestData req)
    {
        var accountLogic = _dynamicsRoutines.GetAccountLogic();     
        var accountList = accountLogic.GetList();
        return AzureFunctionHelper.GetHttpResponseObject(req, HttpStatusCode.OK, accountList);
    }
}

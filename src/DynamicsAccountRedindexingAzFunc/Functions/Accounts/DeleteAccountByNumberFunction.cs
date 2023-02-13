using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


public class DeleteAccountByNumberFunction
{
    private readonly ILogger _logger;
    private readonly DynamicsCrudLogic<AccountEntity> _accountLogic;

    public DeleteAccountByNumberFunction(ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _logger = loggerFactory.CreateLogger<DeleteAccountByNumberFunction>();
        _accountLogic = AzureFunctionHelper.GetEntityLogic<AccountEntity>(_logger, configuration);
    }

    [Function("DeleteAccountByNumberFunction")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "companies/{number}")] HttpRequestData req, int number)
    {
        var accountToDelete = _accountLogic.GetByProperty("new_accountindex", number);
        if (accountToDelete == null) {
            return AzureFunctionHelper.GetHttpResponseObject(req, HttpStatusCode.NotFound);
        }
        _accountLogic.Delete(accountToDelete);
        return AzureFunctionHelper.GetHttpResponseObject(req, HttpStatusCode.NoContent);
    }
}

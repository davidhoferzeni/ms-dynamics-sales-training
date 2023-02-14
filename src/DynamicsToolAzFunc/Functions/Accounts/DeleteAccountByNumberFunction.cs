using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


public class DeleteAccountByNumberFunction
{
    private readonly ILogger _logger;
    private readonly DynamicsRoutines _dynamicsRoutines;

    public DeleteAccountByNumberFunction(ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _logger = loggerFactory.CreateLogger<DeleteAccountByNumberFunction>();
        _dynamicsRoutines = new DynamicsRoutines(configuration, _logger);
    }

    [Function("DeleteAccountByNumberFunction")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "companies/{number}")] HttpRequestData req, int number)
    {
        var accountLogic = _dynamicsRoutines.GetAccountLogic();
        var accountToDelete = accountLogic.GetByProperty("new_accountindex", number);
        if (accountToDelete == null) {
            return AzureFunctionHelper.GetHttpResponseObject(req, HttpStatusCode.NotFound);
        }
        accountLogic.Delete(accountToDelete);
        return AzureFunctionHelper.GetHttpResponseObject(req, HttpStatusCode.NoContent);
    }
}

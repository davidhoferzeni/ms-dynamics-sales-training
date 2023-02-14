using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


public class PostAccountFunction
{
    private readonly ILogger _logger;    
    private readonly DynamicsRoutines _dynamicsRoutines;

    public PostAccountFunction(ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _logger = loggerFactory.CreateLogger<PostAccountFunction>();
        _dynamicsRoutines = new DynamicsRoutines(configuration, _logger);
    }

    [Function("PostAccountFunction")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "companies")] HttpRequestData req)
    {
        string requestBody = String.Empty;
        using (StreamReader streamReader =  new  StreamReader(req.Body))
        {
            requestBody = streamReader.ReadToEnd();
        }
        var accountToSave = JsonSerializer.Deserialize<AccountEntity>(requestBody);
        if (accountToSave == null) {
            var errorMessage = "Request object not valid!";
            return AzureFunctionHelper.GetHttpResponseObject(req, HttpStatusCode.BadRequest, errorMessage);
        }
        var accountLogic = _dynamicsRoutines.GetAccountLogic();
        accountLogic.Create(new [] {accountToSave});
        return AzureFunctionHelper.GetHttpResponseObject(req, HttpStatusCode.OK);
    }
}

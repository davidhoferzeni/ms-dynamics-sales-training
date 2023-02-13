using System.Net;
using System.Reflection.Metadata;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Crm.Sdk;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public static class AzureFunctionHelper
{
    public static HttpResponseData GetHttpResponseObject(HttpRequestData req, HttpStatusCode statusCode = HttpStatusCode.NoContent, object? resultObject = null) {
        var response = req.CreateResponse(HttpStatusCode.OK);
        if (resultObject != null) {
            response.Headers.Add("Content-Type", "application/json");
            response.WriteString(JsonSerializer.Serialize(resultObject));
        }
        return response;
    }

    public static DynamicsCrudLogic<TEntity> GetEntityLogic<TEntity>(ILogger logger, IConfiguration config) where TEntity : class, new() {
        var inputManager = new DynamicsNoninteractiveManager(logger);
        var connectionConfiguration = ConfigurationSectionBuilder<ConnectionConfiguration>.GetConfigurationSection(config, logger, inputManager, "ConnectionConfiguration");
        var session = new DynamicsSession(connectionConfiguration, logger);
        return new DynamicsCrudLogic<TEntity>(session);
    }
}
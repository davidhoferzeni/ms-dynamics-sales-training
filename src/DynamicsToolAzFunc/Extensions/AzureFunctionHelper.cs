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
        var response = req.CreateResponse(statusCode);
        if (resultObject != null) {
            response.Headers.Add("Content-Type", "application/json");
            response.WriteString(JsonSerializer.Serialize(resultObject));
        }
        return response;
    }
}
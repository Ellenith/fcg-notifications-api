using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace fcg_notifications_api;

public class ProcessarPaymentProcessed
{
    private readonly ILogger<ProcessarPaymentProcessed> _logger;

    public ProcessarPaymentProcessed(ILogger<ProcessarPaymentProcessed> logger)
    {
        _logger = logger;
    }

    [Function("ProcessarPaymentProcessed")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}

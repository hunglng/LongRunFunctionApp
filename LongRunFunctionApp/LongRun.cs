using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace LongRunFunctionApp
{
    public class LongRun
    {
        private readonly ILogger<LongRun> _logger;

        public LongRun(ILogger<LongRun> logger)
        {
            _logger = logger;
        }

        [Function(nameof(LongRun))]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous,
                                                nameof(HttpMethods.Get))]
                                    HttpRequest req, [FromQuery] string durationInSeconds)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            if (string.IsNullOrEmpty(durationInSeconds)
                || !int.TryParse(durationInSeconds, out var duration))
            {
                return new OkObjectResult("Welcome to Azure Functions!");
            }
            else
            {
                _logger.LogInformation("Long Running Task will return at {}", 
                    DateTime.Now.AddSeconds(duration));
                await Task.Delay(duration*1000);
                return new OkObjectResult($"Welcome to Azure Functions! After {duration}s");
            }

        }
    }
}

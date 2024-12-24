using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace KaisarionBot;
public class KaisarionBot(ILogger<KaisarionBot> logger)
{
    private readonly ILogger<KaisarionBot> _logger = logger;

    [Function("KaisarionBot")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        var interactionRunner = new InteractionRunner(req, _logger);
        return await interactionRunner.Run();
    }
}

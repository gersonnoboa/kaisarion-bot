using System.Text.Json;
using KaisarionBot.Interactions;
using KaisarionBot.Interactions.View;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KaisarionBot;

public class InteractionRunner(HttpRequest request, ILogger logger)
{
	public async Task<IActionResult> Run()
	{
		logger.LogWarning("Telegram Webhook triggered.");
		string requestBody = await new StreamReader(request.Body).ReadToEndAsync();

		using JsonDocument jsonDoc = JsonDocument.Parse(requestBody);
		JsonElement root = jsonDoc.RootElement;

		logger.LogWarning($"Received payload: {root.ToString()}");

		if (root.TryGetProperty("message", out JsonElement messageElement))
		{
			var text = messageElement.GetProperty("text").GetString() ?? throw new Exception();
			string chatId = messageElement.GetProperty("chat").GetProperty("id").GetRawText();

			switch (text)
			{
				case "/start":
					await StartInteraction.Run(chatId, logger);
					break;

				case "/help":
					await HelpInteraction.Run(chatId, logger);
					break;

				case var t when text.StartsWith("/agregar", StringComparison.OrdinalIgnoreCase):
					var addInteraction = new AddInteraction(logger);
					await addInteraction.Run(chatId, t);
					break;

				case var t when text.StartsWith("/vergerson", StringComparison.OrdinalIgnoreCase):
					var viewInteractionGerson = new ViewInteraction(logger);
					await viewInteractionGerson.Run(chatId, t, User.Gerson);
					break;

				case var t when text.StartsWith("/verjaje", StringComparison.OrdinalIgnoreCase):
					var viewInteractionJaje = new ViewInteraction(logger);
					await viewInteractionJaje.Run(chatId, t, User.Jaje);
					break;

				default:
					await UnknownCommandInteraction.Run(chatId, text, logger);
					break;
			}
		}
		else
		{
			logger.LogWarning("No 'message' property in the webhook payload.");
		}

		// Return HTTP 200 OK
		return new OkObjectResult("Webhook processed");
	}
}

using Microsoft.Extensions.Logging;

namespace KaisarionBot.Interactions;

public class StartInteraction
{
	public static async Task Run(string chatId, ILogger logger)
	{
		var messageToSend = "Hola!";
		var telegramApi = new TelegramApi(logger);
		await telegramApi.Send(chatId, messageToSend, logger);
	}
}

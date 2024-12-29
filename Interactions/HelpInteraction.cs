using Microsoft.Extensions.Logging;

namespace KaisarionBot.Interactions;

public class HelpInteraction
{
	public static async Task Run(string chatId, ILogger logger)
	{
		var messageToSend = "Ayuda";
		var telegramApi = new TelegramApi(logger);
		await telegramApi.Send(chatId, messageToSend, logger);
	}
}

using Microsoft.Extensions.Logging;

namespace KaisarionBot.Interactions;

public class StartInteraction
{
	public static async Task Run(string chatId, ILogger logger)
	{
		var messageToSend = "Hola!";
		await MessageSender.Send(chatId, messageToSend, logger);
	}
}

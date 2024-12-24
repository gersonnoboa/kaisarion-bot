using Microsoft.Extensions.Logging;

namespace KaisarionBot.Interactions;

public class HelpInteraction
{
	public static async Task Run(string chatId, ILogger logger)
	{
		var messageToSend = "Help";
		await MessageSender.Send(chatId, messageToSend, logger);
	}
}

using Microsoft.Extensions.Logging;

namespace KaisarionBot.Interactions;

public class ViewInteraction(ILogger logger)
{
	public async Task Run(string chatId)
	{
		var messageToSend = "Ver";
		await MessageSender.Send(chatId, messageToSend, logger);
	}
}

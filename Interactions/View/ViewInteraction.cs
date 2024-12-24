using KaisarionBot.Interactions.View;
using Microsoft.Extensions.Logging;

namespace KaisarionBot.Interactions;

public class ViewInteraction(ILogger logger)
{
	public async Task Run(string chatId, string interactionText, User user)
	{
		var messageToSend = $"Ver {user.Name}";
		await MessageSender.Send(chatId, messageToSend, logger);
	}
}

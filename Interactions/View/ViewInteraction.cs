using KaisarionBot.Database;
using KaisarionBot.Interactions.View;
using Microsoft.Extensions.Logging;

namespace KaisarionBot.Interactions;

public class ViewInteraction(ILogger logger)
{
	public async Task Run(string chatId, User targetUser)
	{
		var databaseHandler = new DatabaseHandler(logger);
		var result = await databaseHandler.Select(targetUser);

		if (result is not null)
		{
			var messageToSend = "URLs: \n\n" + String.Join("\n", [.. result]);
			await MessageSender.Send(chatId, messageToSend, logger);
		}
		else
		{
			var messageToSend = $"Error obteniendo URLs.";
			await MessageSender.Send(chatId, messageToSend, logger);
		}
	}
}

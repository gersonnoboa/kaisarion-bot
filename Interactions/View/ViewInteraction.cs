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
			foreach (var link in result)
			{
				var keyboard = new
				{
					inline_keyboard = new[]
					{
						new[]
						{
							new { text = "Delete", callback_data = $"delete_id_{link.Id}" },
						}
					}
				};

				await MessageSender.Send(chatId, link.Link, logger, keyboard);
			}
		}
		else
		{
			var messageToSend = $"Error obteniendo URLs.";
			await MessageSender.Send(chatId, messageToSend, logger);
		}
	}
}

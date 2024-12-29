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

		var telegramApi = new TelegramApi(logger);

		if (result is null)
		{
			var messageToSend = $"Error obteniendo URLs.";
			await telegramApi.Send(chatId, messageToSend);
			return;
		}

		if (result.Count == 0)
		{
			await telegramApi.Send(chatId, "No hay links.");
			return;
		}

		foreach (var link in result)
		{
			var keyboard = new
			{
				inline_keyboard = new[]
				{
					new[]
					{
						new { text = "Borrar", callback_data = $"delete_id_{link.Id}" },
					}
				}
			};

			await telegramApi.Send(chatId, link.Link, keyboard);
		}
	}
}

using KaisarionBot.Database;
using Microsoft.Extensions.Logging;

namespace KaisarionBot.Interactions;

class DeleteInteraction
{
	public static async Task Run(string chatId, string messageId, string idToDelete, ILogger logger)
	{
		var databaseHandler = new DatabaseHandler(logger);
		var result = await databaseHandler.Delete(idToDelete);

		var telegramApi = new TelegramApi(logger);

		if (result)
		{
			await telegramApi.Delete(chatId, messageId);
		}
		else
		{
			await telegramApi.Send(chatId, "Error borrando link.", logger);
		}
	}
}

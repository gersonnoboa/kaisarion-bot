using KaisarionBot.Database;
using KaisarionBot.Interactions.View;
using Microsoft.Extensions.Logging;

namespace KaisarionBot.Interactions;

public class AddInteraction(ILogger logger)
{
	public async Task Run(string chatId, string messageText, User user)
	{
		string agregarText = "/agregar";
		string url = messageText[agregarText.Length..].Trim();

		if (string.IsNullOrEmpty(url))
		{
			var messageToSend = "Tienes que poner una URL luego de /agregar. Ejemplo: /agregar https://...";
			await MessageSender.Send(chatId, messageToSend, logger);
		}
		else
		{
			await SaveUrlToDatabaseAsync(chatId, url, user);
		}

	}

	private async Task SaveUrlToDatabaseAsync(string chatId, string url, User user)
	{
		var databaseHandler = new DatabaseHandler(logger);
		var result = await databaseHandler.AddToDatabase(url, user.ChatId);

		if (result)
		{
			var messageToSend = $"URL guardada: '{url}', para user '{user.Name}'";
			await MessageSender.Send(chatId, messageToSend, logger);
		}
		else
		{
			var messageToSend = $"Error guardando URL.";
			await MessageSender.Send(chatId, messageToSend, logger);
		}
	}
}

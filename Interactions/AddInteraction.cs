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

	private async Task SaveUrlToDatabaseAsync(string chatId, string url, User sourceUser)
	{
		var databaseHandler = new DatabaseHandler(logger);
		var targetUser = TargetUser(sourceUser);
		var result = await databaseHandler.AddToDatabase(url, sourceUser, targetUser);

		if (result)
		{
			var messageToSend = $"URL guardada: '{url}' de user {sourceUser.Name} para user {targetUser.Name}.";
			await MessageSender.Send(chatId, messageToSend, logger);
		}
		else
		{
			var messageToSend = $"Error guardando URL.";
			await MessageSender.Send(chatId, messageToSend, logger);
		}
	}

	private User TargetUser(User sourceUser)
	{
		if (sourceUser.ChatId == User.Gerson.ChatId)
		{
			return User.Jaje;
		}
		else if (sourceUser.ChatId == User.Jaje.ChatId)
		{
			return User.Gerson;
		}
		else
		{
			return User.Unknown;
		}
	}
}

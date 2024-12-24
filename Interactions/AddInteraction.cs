using Microsoft.Extensions.Logging;

namespace KaisarionBot.Interactions;

public class AddInteraction(ILogger logger)
{
	public async Task Run(string chatId, string messageText)
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
			await SaveUrlToDatabaseAsync(chatId, url);
		}

	}

	private async Task SaveUrlToDatabaseAsync(string chatId, string url)
	{
		var messageToSend = $"URL guardada: \"{url}\"";
		await MessageSender.Send(chatId, messageToSend, logger);
	}
}

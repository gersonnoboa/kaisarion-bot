using Microsoft.Extensions.Logging;

namespace KaisarionBot.Interactions;

public class UnknownCommandInteraction
{
	public static async Task Run(string chatId, string command, ILogger logger)
	{
		var messageToSend = $"Comand incorrecto: {command}.";
		await MessageSender.Send(chatId, messageToSend, logger);
	}
}

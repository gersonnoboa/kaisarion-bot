using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace KaisarionBot.Interactions;

public class MessageSender
{
	public static async Task Send(string chatId, string messageText, ILogger logger)
	{
		string botToken = Environment.GetEnvironmentVariable("KAISARION_BOT_AUTH_TOKEN") ?? throw new Exception();
		string url = $"https://api.telegram.org/bot{botToken}/sendMessage";

		var payload = new
		{
			chat_id = chatId,
			text = messageText
		};

		string payloadJson = JsonSerializer.Serialize(payload);

		using HttpClient httpClient = new();
		var response = await httpClient.PostAsync(url, new StringContent(payloadJson, Encoding.UTF8, "application/json"));

		if (response.IsSuccessStatusCode)
		{
			logger.LogWarning($"Message {messageText} sent successfully.");
		}
		else
		{
			logger.LogError($"Failed to send message. Status code: {response.StatusCode}");
		}
	}
}

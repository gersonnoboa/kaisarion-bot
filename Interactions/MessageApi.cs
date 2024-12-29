using System.Dynamic;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace KaisarionBot.Interactions;

public class TelegramApi(ILogger logger)
{
	public async Task Send(string chatId, string messageText, object? replyMarkup = null)
	{
		string botToken = Environment.GetEnvironmentVariable("KAISARION_BOT_AUTH_TOKEN") ?? throw new Exception();
		string url = $"https://api.telegram.org/bot{botToken}/sendMessage";

		dynamic payload = new ExpandoObject();
		payload.chat_id = chatId;
		payload.text = messageText;

		if (replyMarkup != null)
		{
			payload.reply_markup = replyMarkup;
		}

		string payloadJson = JsonSerializer.Serialize(payload);

		using HttpClient httpClient = new();
		var response = await httpClient.PostAsync(url, new StringContent(payloadJson, Encoding.UTF8, "application/json"));

		if (response.IsSuccessStatusCode)
		{
			logger.LogWarning($"Message {messageText} sent successfully");
			logger.LogWarning($"Payload JSON: {payloadJson}");
		}
		else
		{
			logger.LogError($"Failed to send message. Status code: {response.StatusCode}");
		}
	}

	public async Task Delete(string chatId, string messageId)
	{
		string botToken = Environment.GetEnvironmentVariable("KAISARION_BOT_AUTH_TOKEN") ?? throw new Exception();
		string url = $"https://api.telegram.org/bot{botToken}/deleteMessage";

		var payload = new
		{
			chat_id = chatId,
			message_id = messageId
		};

		string payloadJson = JsonSerializer.Serialize(payload);

		using HttpClient httpClient = new();
		var response = await httpClient.PostAsync(url, new StringContent(payloadJson, Encoding.UTF8, "application/json"));

		if (response.IsSuccessStatusCode)
		{
			logger.LogWarning($"Message {messageId} deleted successfully");
		}
		else
		{
			logger.LogError($"Failed to delete message. Status code: {response.StatusCode}");
		}
	}
}

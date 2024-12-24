namespace KaisarionBot.Interactions.View;
public class User
{
	private User(string chatId, string name) { ChatId = chatId; Name = name; }
	public string ChatId { get; private set; }
	public string Name { get; private set; }

	public static User Gerson => new("5172080", "Gerson");
	public static User Jaje => new("", "Jaje");
}


namespace KaisarionBot.Interactions.View;
public class User
{
	private User(string chatId, string name) { ChatId = chatId; Name = name; }
	public string ChatId { get; private set; }
	public string Name { get; private set; }

	public static User Gerson => new("5172080", "Gerson");
	public static User Jaje => new("113190984", "Jaje");
	public static User Unknown => new("", "");
	public static User From(string id)
	{
		if (id == Gerson.ChatId) return Gerson;
		else if (id == Jaje.ChatId) return Jaje;
		else return Unknown;
	}
}


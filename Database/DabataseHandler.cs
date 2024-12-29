using KaisarionBot.Interactions.View;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace KaisarionBot.Database;

class DatabaseHandler(ILogger logger)
{
	private readonly string TableName = "Links";
	private readonly string ConnectionString = Environment.GetEnvironmentVariable("KAISARION_BOT_CONNECTION_STRING") ?? throw new Exception();

	public async Task<bool> AddToDatabase(string url, User sourceUser, User targetUser)
	{
		try
		{
			using var connection = new SqlConnection(ConnectionString);
			await connection.OpenAsync();

			var query = $"INSERT INTO [dbo].[{LinksTable.Name}]"
			+ $" ([{LinksTable.SourceUserIdColumnName}], [{LinksTable.TargetUserIdColumnName}], [{LinksTable.LinkColumnName}])"
			+ $" VALUES (@{LinksTable.SourceUserIdColumnName}, @{LinksTable.TargetUserIdColumnName}, @{LinksTable.LinkColumnName});";

			using var command = new SqlCommand(query, connection);
			command.Parameters.AddWithValue($"@{LinksTable.SourceUserIdColumnName}", sourceUser.ChatId);
			command.Parameters.AddWithValue($"@{LinksTable.TargetUserIdColumnName}", targetUser.ChatId);
			command.Parameters.AddWithValue($"@{LinksTable.LinkColumnName}", url);

			await command.ExecuteNonQueryAsync();
			return true;
		}
		catch (Exception ex)
		{
			logger.LogWarning($"Error writing to database: {ex.Message}");
			return false;
		}
	}
}

class LinksTable
{
	public static readonly string Name = "Links";
	public static readonly string SourceUserIdColumnName = "SourceUserId";
	public static readonly string TargetUserIdColumnName = "TargetUserId";
	public static readonly string LinkColumnName = "Link";
}

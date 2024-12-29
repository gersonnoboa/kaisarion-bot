using KaisarionBot.Interactions.View;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace KaisarionBot.Database;

class DatabaseHandler(ILogger logger)
{
	private readonly string TableName = "Links";
	private readonly string ConnectionString = Environment.GetEnvironmentVariable("KAISARION_BOT_CONNECTION_STRING") ?? throw new Exception();

	public async Task<bool> Insert(string url, User sourceUser, User targetUser)
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

	public async Task<List<string>?> Select(User targetUser)
	{
		try
		{
			using var connection = new SqlConnection(ConnectionString);
			await connection.OpenAsync();

			var query = $"SELECT [{LinksTable.LinkColumnName}]"
			+ $" FROM [dbo].[{LinksTable.Name}]"
			+ $" WHERE [{LinksTable.TargetUserIdColumnName}] = @{LinksTable.TargetUserIdColumnName}"
			+ $" ORDER BY [{LinksTable.CreationDateColumnName}] ASC";

			using var command = new SqlCommand(query, connection);
			command.Parameters.AddWithValue($"@{LinksTable.TargetUserIdColumnName}", targetUser.ChatId);

			using var reader = await command.ExecuteReaderAsync();
			var result = new List<string>();
			while (await reader.ReadAsync())
			{
				result.Add(reader.GetString(0));
			}

			return result;

		}
		catch (Exception ex)
		{
			logger.LogWarning($"Error writing to database: {ex.Message}");
			return null;
		}
	}
}

class LinksTable
{
	public static readonly string Name = "Links";
	public static readonly string SourceUserIdColumnName = "SourceUserId";
	public static readonly string TargetUserIdColumnName = "TargetUserId";
	public static readonly string LinkColumnName = "Link";
	public static readonly string CreationDateColumnName = "CreationDate";
}

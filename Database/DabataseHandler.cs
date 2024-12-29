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

			var query = $"INSERT INTO [dbo].[{Links.Name}]"
			+ $" ([{Links.SourceUserIdColumnName}], [{Links.TargetUserIdColumnName}], [{Links.LinkColumnName}])"
			+ $" VALUES (@{Links.SourceUserIdColumnName}, @{Links.TargetUserIdColumnName}, @{Links.LinkColumnName});";

			using var command = new SqlCommand(query, connection);
			command.Parameters.AddWithValue($"@{Links.SourceUserIdColumnName}", sourceUser.ChatId);
			command.Parameters.AddWithValue($"@{Links.TargetUserIdColumnName}", targetUser.ChatId);
			command.Parameters.AddWithValue($"@{Links.LinkColumnName}", url);

			await command.ExecuteNonQueryAsync();
			return true;
		}
		catch (Exception ex)
		{
			logger.LogWarning($"Error writing to database: {ex.Message}");
			return false;
		}
	}

	public async Task<List<Links>?> Select(User targetUser)
	{
		try
		{
			using var connection = new SqlConnection(ConnectionString);
			await connection.OpenAsync();

			var query = $"SELECT [{Links.IdColumnName}], [{Links.LinkColumnName}]"
			+ $" FROM [dbo].[{Links.Name}]"
			+ $" WHERE [{Links.TargetUserIdColumnName}] = @{Links.TargetUserIdColumnName}"
			+ $" ORDER BY [{Links.CreationDateColumnName}] ASC";

			using var command = new SqlCommand(query, connection);
			command.Parameters.AddWithValue($"@{Links.TargetUserIdColumnName}", targetUser.ChatId);

			using var reader = await command.ExecuteReaderAsync();
			var result = new List<Links>();
			while (await reader.ReadAsync())
			{
				var id = reader.GetInt32(0);
				var link = reader.GetString(1);
				result.Add(new Links(id, link));
			}

			return result;

		}
		catch (Exception ex)
		{
			logger.LogWarning($"Error reading from database: {ex.Message}");
			return null;
		}
	}
}


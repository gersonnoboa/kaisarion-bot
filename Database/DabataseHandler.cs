using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace KaisarionBot.Database;

class DatabaseHandler(ILogger logger)
{
	private readonly string TableName = "Links";
	private readonly string ConnectionString = Environment.GetEnvironmentVariable("KAISARION_BOT_CONNECTION_STRING") ?? throw new Exception();

	public async Task<bool> AddToDatabase(string url, string user)
	{
		try
		{
			using var connection = new SqlConnection(ConnectionString);
			await connection.OpenAsync();

			var query = $"INSERT INTO [dbo].[{LinksTable.Name}] ([{LinksTable.UserColumnName}], [{LinksTable.LinkColumnName}])" +
			$" VALUES (@{LinksTable.UserColumnName}, @{LinksTable.LinkColumnName});";

			using var command = new SqlCommand(query, connection);
			command.Parameters.AddWithValue($"@{LinksTable.UserColumnName}", user);
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
	public static readonly string UserColumnName = "UserId";
	public static readonly string LinkColumnName = "Link";
}

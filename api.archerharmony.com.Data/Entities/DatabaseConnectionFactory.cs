using MySqlConnector;

namespace api.archerharmony.com.Entities.Entities;

public interface IDatabaseConnectionFactory
{
    MySqlConnection CreateConnection(DatabaseType databaseType);
}

public class DatabaseConnectionFactory(Dictionary<DatabaseType, string> connectionStrings) : IDatabaseConnectionFactory
{
    public MySqlConnection CreateConnection(DatabaseType databaseType)
    {
        if (!connectionStrings.TryGetValue(databaseType, out var connectionString))
        {
            throw new InvalidOperationException($"Connection string not found for database: {databaseType}");
        }

        return new MySqlConnection(connectionString);
    }
}
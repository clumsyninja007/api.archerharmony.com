using System.Data;
using Dapper;

namespace api.archerharmony.com.Extensions;

public static class DapperExtensions
{
    extension(IDbConnection connection)
    {
        public Task<IEnumerable<T>> QueryAsync<T>(string sql,
            object? param = null,
            CancellationToken cancellationToken = default)
        {
            var command = new CommandDefinition(sql, param, cancellationToken: cancellationToken);
            return connection.QueryAsync<T>(command);
        }

        public Task<T?> QueryFirstOrDefaultAsync<T>(string sql,
            object? param = null,
            CancellationToken cancellationToken = default)
        {
            var command = new CommandDefinition(sql, param, cancellationToken: cancellationToken);
            return connection.QueryFirstOrDefaultAsync<T>(command);
        }

        public Task<T> QueryFirstAsync<T>(string sql,
            object? param = null,
            CancellationToken cancellationToken = default)
        {
            var command = new CommandDefinition(sql, param, cancellationToken: cancellationToken);
            return connection.QueryFirstAsync<T>(command);
        }

        public Task<T?> QuerySingleOrDefaultAsync<T>(string sql,
            object? param = null,
            CancellationToken cancellationToken = default)
        {
            var command = new CommandDefinition(sql, param, cancellationToken: cancellationToken);
            return connection.QuerySingleOrDefaultAsync<T>(command);
        }

        public Task<T> QuerySingleAsync<T>(string sql,
            object? param = null,
            CancellationToken cancellationToken = default)
        {
            var command = new CommandDefinition(sql, param, cancellationToken: cancellationToken);
            return connection.QuerySingleAsync<T>(command);
        }

        public Task<int> ExecuteAsync(string sql,
            object? param = null,
            CancellationToken cancellationToken = default)
        {
            var command = new CommandDefinition(sql, param, cancellationToken: cancellationToken);
            return connection.ExecuteAsync(command);
        }

        public Task<T?> ExecuteScalarAsync<T>(string sql,
            object? param = null,
            CancellationToken cancellationToken = default)
        {
            var command = new CommandDefinition(sql, param, cancellationToken: cancellationToken);
            return connection.ExecuteScalarAsync<T>(command);
        }

        public Task<SqlMapper.GridReader> QueryMultipleAsync(string sql,
            object? param = null,
            CancellationToken cancellationToken = default)
        {
            var command = new CommandDefinition(sql, param, cancellationToken: cancellationToken);
            return connection.QueryMultipleAsync(command);
        }
    }
}
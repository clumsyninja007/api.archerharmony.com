using api.archerharmony.com.Entities.Entities;
using Dapper;

namespace api.archerharmony.com.Features.Hoelterling.GetPersonalInfo;

public interface IData
{
    Task<Response?> GetPersonalInfo(int id);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(IDatabaseConnectionFactory connectionFactory) : IData
{
    public async Task<Response?> GetPersonalInfo(int id)
    {
        const string query = "SELECT name, title FROM person WHERE id = @id";
        await using var conn = connectionFactory.CreateConnection(DatabaseType.Hoelterling);
        return await conn.QueryFirstOrDefaultAsync<Response>(query, new { id });
    }
}
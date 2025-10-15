using api.archerharmony.com.Entities.Entities;
using Dapper;

namespace api.archerharmony.com.Features.Hoelterling.GetSkills;

public interface IData
{
    public Task<List<string>> GetSkills(int personId);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(IDatabaseConnectionFactory databaseConnectionFactory) : IData
{
    public async Task<List<string>> GetSkills(int personId)
    {
        await using var conn = databaseConnectionFactory.CreateConnection(DatabaseType.Hoelterling);

        const string query =
            """
            SELECT label
            FROM skills
            WHERE person_id = @personId
            """;

        var skills = await conn.QueryAsync<string>(query, new { personId });
        return skills.ToList();
    }
}
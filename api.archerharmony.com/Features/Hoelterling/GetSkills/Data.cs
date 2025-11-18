using api.archerharmony.com.Entities.Entities;
using api.archerharmony.com.Extensions;

namespace api.archerharmony.com.Features.Hoelterling.GetSkills;

public interface IData
{
    public Task<List<string>> GetSkills(int personId, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(IDatabaseConnectionFactory databaseConnectionFactory) : IData
{
    public async Task<List<string>> GetSkills(int personId, CancellationToken ct = default)
    {
        await using var conn = databaseConnectionFactory.CreateConnection(DatabaseType.Hoelterling);

        const string query =
            """
            SELECT label
            FROM skills
            WHERE person_id = @personId
            """;

        var skills = await conn.QueryAsync<string>(query, new { personId }, cancellationToken: ct);
        return skills.ToList();
    }
}
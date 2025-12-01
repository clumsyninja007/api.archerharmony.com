using api.archerharmony.com.Entities.Entities;
using api.archerharmony.com.Extensions;

namespace api.archerharmony.com.Features.Hoelterling.GetSkills;

public interface IData
{
    public Task<List<string>> GetSkills(int personId, string language, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(IDatabaseConnectionFactory databaseConnectionFactory) : IData
{
    public async Task<List<string>> GetSkills(int personId, string language, CancellationToken ct = default)
    {
        await using var conn = databaseConnectionFactory.CreateConnection(DatabaseType.Hoelterling);

        const string query =
            """
            SELECT COALESCE(sl.label, s.label) AS label
            FROM skills s
            LEFT JOIN skills_localized sl
                ON s.id = sl.skill_id
                AND sl.language_code = @language
            WHERE s.person_id = @personId
            """;

        var skills = await conn.QueryAsync<string>(query, new { personId, language }, cancellationToken: ct);
        return skills.ToList();
    }
}
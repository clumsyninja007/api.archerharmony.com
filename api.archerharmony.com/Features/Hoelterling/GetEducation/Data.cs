using api.archerharmony.com.Entities.Entities;
using Dapper;

namespace api.archerharmony.com.Features.Hoelterling.GetEducation;

public interface IData
{
    Task<List<EducationRecord>> GetEducation(int personId, string language, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(IDatabaseConnectionFactory databaseConnectionFactory) : IData
{
    public async Task<List<EducationRecord>> GetEducation(int personId, string language, CancellationToken ct = default)
    {
        await using var conn = databaseConnectionFactory.CreateConnection(DatabaseType.Hoelterling);

        const string query =
            """
            SELECT
                COALESCE(el.school, e.school) AS school,
                COALESCE(el.degree_earned, e.degree_earned) AS "degree",
                COALESCE(el.major, e.major) AS major,
                e.start_date AS startDate,
                e.end_date AS endDate
            FROM education e
            LEFT JOIN education_localized el
                ON e.id = el.education_id
                AND el.language_code = @language
            WHERE e.person_id = @personId
            """;

        var command = new CommandDefinition(query, new { personId, language }, cancellationToken: ct);
        var response = await conn.QueryAsync<EducationRecord>(command);
        return response.ToList();
    }
}
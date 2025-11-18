using api.archerharmony.com.Entities.Entities;
using api.archerharmony.com.Extensions;

namespace api.archerharmony.com.Features.Hoelterling.GetEducation;

public interface IData
{
    Task<List<EducationRecord>> GetEducation(int personId, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(IDatabaseConnectionFactory databaseConnectionFactory) : IData
{
    public async Task<List<EducationRecord>> GetEducation(int personId, CancellationToken ct = default)
    {
        await using var conn = databaseConnectionFactory.CreateConnection(DatabaseType.Hoelterling);

        const string query =
            """
            SELECT school
                ,degree_earned AS "degree"
                ,major
                ,start_date AS startDate
                ,end_date AS endDate
            FROM education
            WHERE person_id = @personId
            """;

        var response = await conn.QueryAsync<EducationRecord>(query, new { personId }, cancellationToken: ct);
        return response.ToList();
    }
}
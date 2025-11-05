using api.archerharmony.com.Entities.Entities;
using Dapper;

namespace api.archerharmony.com.Features.Hoelterling.GetEducation;

public interface IData
{
    Task<List<EducationRecord>> GetEducation(int personId);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(IDatabaseConnectionFactory databaseConnectionFactory) : IData
{
    public async Task<List<EducationRecord>> GetEducation(int personId)
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

        var response = await conn.QueryAsync<EducationRecord>(query, new { personId });
        return response.ToList();
    }
}
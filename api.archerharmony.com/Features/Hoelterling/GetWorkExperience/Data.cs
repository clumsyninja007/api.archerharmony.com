using api.archerharmony.com.Entities.Entities;
using Dapper;

namespace api.archerharmony.com.Features.Hoelterling.GetWorkExperience;

public interface IData
{
    public Task<List<WorkExperience>> GetWorkExperiences(int personId);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(IDatabaseConnectionFactory databaseConnectionFactory) : IData
{
    public async Task<List<WorkExperience>> GetWorkExperiences(int personId)
    {
        await using var conn = databaseConnectionFactory.CreateConnection(DatabaseType.Hoelterling);
        
        const string sql =
            """
            SELECT 
                id,
                title,
                company,
                location,
                start_date AS StartDate,
                end_date AS EndDate
            FROM work_experience
            WHERE person_id = @PersonId;

            SELECT 
                work_experience_id AS WorkExperienceId,
                skill
            FROM work_experience_skills
            WHERE work_experience_id IN (
                SELECT id FROM work_experience WHERE person_id = @PersonId
            );
            """;

        await using var multi = await conn.QueryMultipleAsync(sql, new { PersonId = personId });

        var experiences = (await multi.ReadAsync<WorkExperienceRow>()).ToList();
        var skills = (await multi.ReadAsync<SkillRow>()).ToList();

        // join them
        var result = experiences
            .Select(exp => new WorkExperience(
                exp.Title,
                exp.Company,
                exp.Location,
                exp.StartDate,
                exp.EndDate,
                skills.Where(s => s.WorkExperienceId == exp.Id)
                    .Select(s => s.Skill)
                    .ToList()
            ))
            .OrderByDescending(exp => exp.StartDate)
            .ToList();

        return result;
    }
}

internal record WorkExperienceRow(
    int Id,
    string Title,
    string Company,
    string Location,
    DateTime StartDate,
    DateTime? EndDate);

internal record SkillRow(int WorkExperienceId, string Skill);

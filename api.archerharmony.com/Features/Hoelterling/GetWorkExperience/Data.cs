using api.archerharmony.com.Entities.Entities;
using Dapper;

namespace api.archerharmony.com.Features.Hoelterling.GetWorkExperience;

public interface IData
{
    public Task<List<WorkExperience>> GetWorkExperiences(int personId, string language, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(IDatabaseConnectionFactory databaseConnectionFactory) : IData
{
    public async Task<List<WorkExperience>> GetWorkExperiences(int personId, string language, CancellationToken ct = default)
    {
        await using var conn = databaseConnectionFactory.CreateConnection(DatabaseType.Hoelterling);

        const string sql =
            """
            SELECT
                we.id,
                COALESCE(wel.title, we.title) AS title,
                we.company AS company,
                COALESCE(wel.location, we.location) AS location,
                we.start_date AS StartDate,
                we.end_date AS EndDate
            FROM work_experience we
            LEFT JOIN work_experience_localized wel
                ON we.id = wel.work_experience_id
                AND wel.language_code = @Language
            WHERE we.person_id = @PersonId;

            SELECT
                wes.work_experience_id AS WorkExperienceId,
                COALESCE(wesl.skill, wes.skill) AS skill,
                wes.display_order AS DisplayOrder
            FROM work_experience_skills wes
            LEFT JOIN work_experience_skills_localized wesl
                ON wes.id = wesl.work_experience_skill_id
                AND wesl.language_code = @Language
            WHERE wes.work_experience_id IN (
                SELECT id FROM work_experience WHERE person_id = @PersonId
            )
            ORDER BY wes.display_order;
            """;

        var command = new CommandDefinition(sql, new { PersonId = personId, Language = language }, cancellationToken: ct);
        await using var multi = await conn.QueryMultipleAsync(command);

        var experiences = (await multi.ReadAsync<WorkExperienceRow>()).ToList();
        var skills = (await multi.ReadAsync<SkillRow>()).ToList();

        // join them
        var result = experiences
            .Select(exp => new WorkExperience(
                exp.Id,
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

internal record SkillRow(int WorkExperienceId, string Skill, int DisplayOrder);

using api.archerharmony.com.Entities.Entities;
using Dapper;

namespace api.archerharmony.com.Features.Hoelterling.DeleteWorkExperience;

public interface IData
{
    Task DeleteWorkExperience(int experienceId);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(IDatabaseConnectionFactory connectionFactory) : IData
{
    public async Task DeleteWorkExperience(int experienceId)
    {
        await using var conn = connectionFactory.CreateConnection(DatabaseType.Hoelterling);
        await conn.OpenAsync();
        await using var transaction = await conn.BeginTransactionAsync();

        try
        {
            // Delete skills localizations
            const string deleteSkillsLocalized = """
                DELETE wesl FROM work_experience_skills_localized wesl
                INNER JOIN work_experience_skills wes ON wesl.work_experience_skill_id = wes.id
                WHERE wes.work_experience_id = @ExperienceId
                """;

            await conn.ExecuteAsync(deleteSkillsLocalized, new { ExperienceId = experienceId }, transaction);

            // Delete skills
            const string deleteSkills = """
                DELETE FROM work_experience_skills
                WHERE work_experience_id = @ExperienceId
                """;

            await conn.ExecuteAsync(deleteSkills, new { ExperienceId = experienceId }, transaction);

            // Delete localized data
            const string deleteLocalized = """
                DELETE FROM work_experience_localized
                WHERE work_experience_id = @ExperienceId
                """;

            await conn.ExecuteAsync(deleteLocalized, new { ExperienceId = experienceId }, transaction);

            // Delete base work experience
            const string deleteBase = """
                DELETE FROM work_experience
                WHERE id = @ExperienceId
                """;

            await conn.ExecuteAsync(deleteBase, new { ExperienceId = experienceId }, transaction);

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
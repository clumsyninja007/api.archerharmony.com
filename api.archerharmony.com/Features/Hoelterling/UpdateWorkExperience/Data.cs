using api.archerharmony.com.Entities.Entities;
using Dapper;

namespace api.archerharmony.com.Features.Hoelterling.UpdateWorkExperience;

public interface IData
{
    Task UpdateWorkExperience(int experienceId, WorkExperienceData en, WorkExperienceData de, string updatedBy);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(IDatabaseConnectionFactory connectionFactory) : IData
{
    public async Task UpdateWorkExperience(int experienceId, WorkExperienceData en, WorkExperienceData de, string updatedBy)
    {
        await using var conn = connectionFactory.CreateConnection(DatabaseType.Hoelterling);
        await conn.OpenAsync();
        await using var transaction = await conn.BeginTransactionAsync();

        try
        {
            // Update English in base work_experience table
            const string updateBase = """
                UPDATE work_experience
                SET title = @Title,
                    company = @Company,
                    location = @Location,
                    start_date = @StartDate,
                    end_date = @EndDate,
                    updated_by = @UpdatedBy,
                    updated_at = NOW()
                WHERE id = @ExperienceId
                """;

            await conn.ExecuteAsync(updateBase, new
            {
                ExperienceId = experienceId,
                Title = en.Title,
                Company = en.Company,
                Location = en.Location,
                StartDate = en.StartDate,
                EndDate = en.EndDate,
                UpdatedBy = updatedBy
            }, transaction);

            // Upsert German localized data only (company name is not localized as it's a proper noun)
            const string upsertDe = """
                INSERT INTO work_experience_localized (work_experience_id, language_code, title, location, updated_by)
                VALUES (@ExperienceId, 'de', @Title, @Location, @UpdatedBy)
                ON DUPLICATE KEY UPDATE
                    title = @Title,
                    location = @Location,
                    updated_by = @UpdatedBy,
                    updated_at = NOW()
                """;

            await conn.ExecuteAsync(upsertDe, new
            {
                ExperienceId = experienceId,
                Title = de.Title,
                Location = de.Location,
                UpdatedBy = updatedBy
            }, transaction);

            // Delete all existing skills and their localizations
            const string deleteSkillsLocalized = """
                DELETE wesl FROM work_experience_skills_localized wesl
                INNER JOIN work_experience_skills wes ON wesl.work_experience_skill_id = wes.id
                WHERE wes.work_experience_id = @ExperienceId
                """;

            await conn.ExecuteAsync(deleteSkillsLocalized, new { ExperienceId = experienceId }, transaction);

            const string deleteSkills = """
                DELETE FROM work_experience_skills
                WHERE work_experience_id = @ExperienceId
                """;

            await conn.ExecuteAsync(deleteSkills, new { ExperienceId = experienceId }, transaction);

            // Insert new skills
            for (var i = 0; i < en.Skills.Count; i++)
            {
                // Insert English skill into base table
                const string insertSkill = """
                    INSERT INTO work_experience_skills (work_experience_id, skill, display_order, updated_by, created_at, updated_at)
                    VALUES (@ExperienceId, @Skill, @DisplayOrder, @UpdatedBy, NOW(), NOW());
                    SELECT LAST_INSERT_ID();
                    """;

                var skillId = await conn.ExecuteScalarAsync<int>(insertSkill, new
                {
                    ExperienceId = experienceId,
                    Skill = en.Skills[i],
                    DisplayOrder = i,
                    UpdatedBy = updatedBy
                }, transaction);

                // Insert German skill localization only
                if (i < de.Skills.Count)
                {
                    const string insertSkillDe = """
                        INSERT INTO work_experience_skills_localized (work_experience_skill_id, language_code, skill, updated_by, created_at, updated_at)
                        VALUES (@SkillId, 'de', @Skill, @UpdatedBy, NOW(), NOW())
                        """;

                    await conn.ExecuteAsync(insertSkillDe, new
                    {
                        SkillId = skillId,
                        Skill = de.Skills[i],
                        UpdatedBy = updatedBy
                    }, transaction);
                }
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
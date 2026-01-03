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
            // Update base work_experience table
            const string updateBase = """
                UPDATE work_experience
                SET company = @Company,
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
                Company = en.Company,
                Location = en.Location,
                StartDate = en.StartDate,
                EndDate = en.EndDate,
                UpdatedBy = updatedBy
            }, transaction);

            // Upsert localized data for English
            const string upsertEn = """
                INSERT INTO work_experience_localized (work_experience_id, language_code, title, updated_by)
                VALUES (@ExperienceId, 'en', @Title, @UpdatedBy)
                ON DUPLICATE KEY UPDATE
                    title = @Title,
                    updated_by = @UpdatedBy,
                    updated_at = NOW()
                """;

            await conn.ExecuteAsync(upsertEn, new
            {
                ExperienceId = experienceId,
                Title = en.Title,
                UpdatedBy = updatedBy
            }, transaction);

            // Upsert localized data for German
            const string upsertDe = """
                INSERT INTO work_experience_localized (work_experience_id, language_code, title, updated_by)
                VALUES (@ExperienceId, 'de', @Title, @UpdatedBy)
                ON DUPLICATE KEY UPDATE
                    title = @Title,
                    updated_by = @UpdatedBy,
                    updated_at = NOW()
                """;

            await conn.ExecuteAsync(upsertDe, new
            {
                ExperienceId = experienceId,
                Title = de.Title,
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
                const string insertSkill = """
                    INSERT INTO work_experience_skills (work_experience_id, skill_name, display_order, updated_by, created_at, updated_at)
                    VALUES (@ExperienceId, @SkillName, @DisplayOrder, @UpdatedBy, NOW(), NOW());
                    SELECT LAST_INSERT_ID();
                    """;

                var skillId = await conn.ExecuteScalarAsync<int>(insertSkill, new
                {
                    ExperienceId = experienceId,
                    SkillName = en.Skills[i],
                    DisplayOrder = i,
                    UpdatedBy = updatedBy
                }, transaction);

                // Insert English skill localization
                const string insertSkillEn = """
                    INSERT INTO work_experience_skills_localized (work_experience_skill_id, language_code, skill_name, updated_by, created_at, updated_at)
                    VALUES (@SkillId, 'en', @SkillName, @UpdatedBy, NOW(), NOW())
                    """;

                await conn.ExecuteAsync(insertSkillEn, new
                {
                    SkillId = skillId,
                    SkillName = en.Skills[i],
                    UpdatedBy = updatedBy
                }, transaction);

                // Insert German skill localization
                if (i < de.Skills.Count)
                {
                    const string insertSkillDe = """
                        INSERT INTO work_experience_skills_localized (work_experience_skill_id, language_code, skill_name, updated_by, created_at, updated_at)
                        VALUES (@SkillId, 'de', @SkillName, @UpdatedBy, NOW(), NOW())
                        """;

                    await conn.ExecuteAsync(insertSkillDe, new
                    {
                        SkillId = skillId,
                        SkillName = de.Skills[i],
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
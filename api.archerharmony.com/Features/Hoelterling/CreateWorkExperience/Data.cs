using api.archerharmony.com.Entities.Entities;
using Dapper;

namespace api.archerharmony.com.Features.Hoelterling.CreateWorkExperience;

public interface IData
{
    Task<int> CreateWorkExperience(int personId, WorkExperienceData en, WorkExperienceData de, string createdBy);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(IDatabaseConnectionFactory connectionFactory) : IData
{
    public async Task<int> CreateWorkExperience(int personId, WorkExperienceData en, WorkExperienceData de, string createdBy)
    {
        await using var conn = connectionFactory.CreateConnection(DatabaseType.Hoelterling);
        await conn.OpenAsync();
        await using var transaction = await conn.BeginTransactionAsync();

        try
        {
            // Insert into base work_experience table
            const string insertBase = """
                INSERT INTO work_experience (person_id, company, location, start_date, end_date, updated_by, created_at, updated_at)
                VALUES (@PersonId, @Company, @Location, @StartDate, @EndDate, @UpdatedBy, NOW(), NOW());
                SELECT LAST_INSERT_ID();
                """;

            var experienceId = await conn.ExecuteScalarAsync<int>(insertBase, new
            {
                PersonId = personId,
                Company = en.Company,
                Location = en.Location,
                StartDate = en.StartDate,
                EndDate = en.EndDate,
                UpdatedBy = createdBy
            }, transaction);

            // Insert localized data for English
            const string insertEn = """
                INSERT INTO work_experience_localized (work_experience_id, language_code, title, updated_by, created_at, updated_at)
                VALUES (@ExperienceId, 'en', @Title, @UpdatedBy, NOW(), NOW())
                """;

            await conn.ExecuteAsync(insertEn, new
            {
                ExperienceId = experienceId,
                Title = en.Title,
                UpdatedBy = createdBy
            }, transaction);

            // Insert localized data for German
            const string insertDe = """
                INSERT INTO work_experience_localized (work_experience_id, language_code, title, updated_by, created_at, updated_at)
                VALUES (@ExperienceId, 'de', @Title, @UpdatedBy, NOW(), NOW())
                """;

            await conn.ExecuteAsync(insertDe, new
            {
                ExperienceId = experienceId,
                Title = de.Title,
                UpdatedBy = createdBy
            }, transaction);

            // Insert skills
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
                    UpdatedBy = createdBy
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
                    UpdatedBy = createdBy
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
                        UpdatedBy = createdBy
                    }, transaction);
                }
            }

            await transaction.CommitAsync();
            return experienceId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
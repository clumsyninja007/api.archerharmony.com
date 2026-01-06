using api.archerharmony.com.Entities.Entities;
using Dapper;

namespace api.archerharmony.com.Features.Hoelterling.UpdateProject;

public interface IData
{
    Task UpdateProject(int personId, int projectId, ProjectData en, ProjectData de, string updatedBy);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(IDatabaseConnectionFactory connectionFactory) : IData
{
    public async Task UpdateProject(int personId, int projectId, ProjectData en, ProjectData de, string updatedBy)
    {
        await using var conn = connectionFactory.CreateConnection(DatabaseType.Hoelterling);
        await conn.OpenAsync();
        await using var transaction = await conn.BeginTransactionAsync();

        try
        {
            // Update English data in base project table
            const string updateBase = """
                UPDATE project
                SET title = @Title,
                    description = @Description,
                    long_description = @LongDescription,
                    image_url = @ImageUrl,
                    live_url = @LiveUrl,
                    demo_url = @DemoUrl,
                    github_url = @GithubUrl,
                    display_order = @DisplayOrder,
                    updated_by = @UpdatedBy,
                    updated_at = NOW()
                WHERE id = @ProjectId AND person_id = @PersonId
                """;

            await conn.ExecuteAsync(updateBase, new
            {
                ProjectId = projectId,
                PersonId = personId,
                Title = en.Title,
                Description = en.Description,
                LongDescription = en.LongDescription,
                ImageUrl = en.ImageUrl,
                LiveUrl = en.LiveUrl,
                DemoUrl = en.DemoUrl,
                GithubUrl = en.GithubUrl,
                DisplayOrder = en.DisplayOrder,
                UpdatedBy = updatedBy
            }, transaction);

            // Upsert German localized data
            const string upsertDe = """
                INSERT INTO project_localized (project_id, language_code, title, description, long_description, updated_at)
                VALUES (@ProjectId, 'de', @Title, @Description, @LongDescription, NOW())
                ON DUPLICATE KEY UPDATE
                    title = @Title,
                    description = @Description,
                    long_description = @LongDescription,
                    updated_at = NOW()
                """;

            await conn.ExecuteAsync(upsertDe, new
            {
                ProjectId = projectId,
                Title = de.Title,
                Description = de.Description,
                LongDescription = de.LongDescription
            }, transaction);

            // Delete existing technologies
            const string deleteTechs = """
                DELETE FROM project_technology
                WHERE project_id = @ProjectId
                """;

            await conn.ExecuteAsync(deleteTechs, new { ProjectId = projectId }, transaction);

            // Insert updated technologies
            for (var i = 0; i < en.Technologies.Count; i++)
            {
                // Insert English technology
                const string insertTech = """
                    INSERT INTO project_technology (project_id, technology, display_order, created_at, updated_at)
                    VALUES (@ProjectId, @Technology, @DisplayOrder, NOW(), NOW());
                    SELECT LAST_INSERT_ID();
                    """;

                var techId = await conn.ExecuteScalarAsync<int>(insertTech, new
                {
                    ProjectId = projectId,
                    Technology = en.Technologies[i],
                    DisplayOrder = i
                }, transaction);

                // Insert German technology localization
                if (i < de.Technologies.Count)
                {
                    const string insertTechDe = """
                        INSERT INTO project_technology_localized (project_technology_id, language_code, technology, created_at, updated_at)
                        VALUES (@TechId, 'de', @Technology, NOW(), NOW())
                        """;

                    await conn.ExecuteAsync(insertTechDe, new
                    {
                        TechId = techId,
                        Technology = de.Technologies[i]
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

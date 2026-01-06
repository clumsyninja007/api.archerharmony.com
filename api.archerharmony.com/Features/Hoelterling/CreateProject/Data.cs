using api.archerharmony.com.Entities.Entities;
using Dapper;

namespace api.archerharmony.com.Features.Hoelterling.CreateProject;

public interface IData
{
    Task<int> CreateProject(int personId, ProjectData en, ProjectData de, string createdBy);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(IDatabaseConnectionFactory connectionFactory) : IData
{
    public async Task<int> CreateProject(int personId, ProjectData en, ProjectData de, string createdBy)
    {
        await using var conn = connectionFactory.CreateConnection(DatabaseType.Hoelterling);
        await conn.OpenAsync();
        await using var transaction = await conn.BeginTransactionAsync();

        try
        {
            // Insert English into base project table
            const string insertBase = """
                INSERT INTO project (person_id, title, description, long_description, image_url, live_url, demo_url, github_url, display_order, updated_by, created_at, updated_at)
                VALUES (@PersonId, @Title, @Description, @LongDescription, @ImageUrl, @LiveUrl, @DemoUrl, @GithubUrl, @DisplayOrder, @UpdatedBy, NOW(), NOW());
                SELECT LAST_INSERT_ID();
                """;

            var projectId = await conn.ExecuteScalarAsync<int>(insertBase, new
            {
                PersonId = personId,
                Title = en.Title,
                Description = en.Description,
                LongDescription = en.LongDescription,
                ImageUrl = en.ImageUrl,
                LiveUrl = en.LiveUrl,
                DemoUrl = en.DemoUrl,
                GithubUrl = en.GithubUrl,
                DisplayOrder = en.DisplayOrder,
                UpdatedBy = createdBy
            }, transaction);

            // Insert German localized data
            const string insertDe = """
                INSERT INTO project_localized (project_id, language_code, title, description, long_description, created_at, updated_at)
                VALUES (@ProjectId, 'de', @Title, @Description, @LongDescription, NOW(), NOW())
                """;

            await conn.ExecuteAsync(insertDe, new
            {
                ProjectId = projectId,
                Title = de.Title,
                Description = de.Description,
                LongDescription = de.LongDescription
            }, transaction);

            // Insert technologies
            for (var i = 0; i < en.Technologies.Count; i++)
            {
                // Insert English technology into base table
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
            return projectId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}

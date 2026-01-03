using api.archerharmony.com.Entities.Entities;
using Dapper;

namespace api.archerharmony.com.Features.Hoelterling.GetProjects;

public interface IData
{
    public Task<List<Project>> GetProjects(int personId, string language, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(IDatabaseConnectionFactory databaseConnectionFactory) : IData
{
    public async Task<List<Project>> GetProjects(int personId, string language, CancellationToken ct = default)
    {
        await using var conn = databaseConnectionFactory.CreateConnection(DatabaseType.Hoelterling);

        const string sql =
            """
            SELECT
                p.id,
                COALESCE(pl.title, p.title) AS title,
                COALESCE(pl.description, p.description) AS description,
                COALESCE(pl.long_description, p.long_description) AS longDescription,
                p.image_url AS imageUrl,
                p.live_url AS liveUrl,
                p.github_url AS githubUrl,
                p.display_order AS displayOrder
            FROM project p
            LEFT JOIN project_localized pl
                ON p.id = pl.project_id
                AND pl.language_code = @language
            WHERE p.person_id = @personId
                AND p.is_active = TRUE
            ORDER BY p.display_order ASC;

            SELECT
                pt.project_id AS projectId,
                COALESCE(ptl.technology, pt.technology) AS technology,
                pt.display_order AS displayOrder
            FROM project_technology pt
            LEFT JOIN project_technology_localized ptl
                ON pt.id = ptl.project_technology_id
                AND ptl.language_code = @language
            WHERE pt.project_id IN (
                SELECT id FROM project WHERE person_id = @personId AND is_active = TRUE
            )
            ORDER BY pt.display_order ASC;
            """;

        var command = new CommandDefinition(sql, new { personId, language }, cancellationToken: ct);
        await using var multi = await conn.QueryMultipleAsync(command);

        var projects = (await multi.ReadAsync<ProjectRow>()).ToList();
        var technologies = (await multi.ReadAsync<TechnologyRow>()).ToList();

        var result = projects
            .Select(proj => new Project(
                proj.Id,
                proj.Title,
                proj.Description,
                proj.LongDescription,
                technologies.Where(t => t.ProjectId == proj.Id)
                    .OrderBy(t => t.DisplayOrder)
                    .Select(t => t.Technology)
                    .ToList(),
                proj.ImageUrl,
                proj.LiveUrl,
                proj.GithubUrl,
                proj.DisplayOrder
            ))
            .ToList();

        return result;
    }
}

internal record ProjectRow(
    int Id,
    string Title,
    string Description,
    string LongDescription,
    string? ImageUrl,
    string? LiveUrl,
    string? GithubUrl,
    int DisplayOrder);

internal record TechnologyRow(int ProjectId, string Technology, int DisplayOrder);
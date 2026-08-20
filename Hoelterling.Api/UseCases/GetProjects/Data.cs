using FastEndpoints;
using Hoelterling.Api.Extensions;
using Hoelterling.Api.Helpers;
using Microsoft.Azure.Cosmos;

namespace Hoelterling.Api.UseCases.GetProjects;

public interface IData
{
    public Task<List<Project>> GetProjects(int personId, string language, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(Container container) : IData
{
    public async Task<List<Project>> GetProjects(int personId, string language, CancellationToken ct = default)
    {
        const string query =
            """
            SELECT c.id
                ,c.title
                ,c.description
                ,c.longDescription
                ,c.imageUrl
                ,c.liveUrl
                ,c.demoUrl
                ,c.githubUrl
                ,c.displayOrder
                ,c.technologies
                ,c.localizations
            FROM c
            WHERE c.type = "project"
                AND c.personId = @personId
                AND c.isActive = true
            ORDER BY c.displayOrder
            """;

        var docs = await container.QueryAsync<ProjectDoc>(
            new QueryDefinition(query).WithParameter("@personId", personId),
            new PartitionKey("project"), ct);

        return docs.Select(doc => new Project(
            Id: doc.Id,
            Title: LocalizationHelper.Localize(doc.Title, doc.Localizations, language, "title"),
            Description: LocalizationHelper.Localize(doc.Description, doc.Localizations, language, "description"),
            LongDescription: LocalizationHelper.Localize(doc.LongDescription, doc.Localizations, language, "longDescription"),
            Technologies: doc.Technologies
                .OrderBy(t => t.DisplayOrder)
                .Select(t => LocalizationHelper.Localize(t.Technology, t.Localizations, language, "technology")),
            ImageUrl: doc.ImageUrl,
            LiveUrl: doc.LiveUrl,
            DemoUrl: doc.DemoUrl,
            GithubUrl: doc.GithubUrl,
            DisplayOrder: doc.DisplayOrder)
        ).ToList();
    }
}

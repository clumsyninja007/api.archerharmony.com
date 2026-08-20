using FastEndpoints;
using Hoelterling.Api.UseCases.Projects.Dtos;
using Microsoft.Azure.Cosmos;

namespace Hoelterling.Api.UseCases.Projects.UpdateProject;

public interface IData
{
    Task UpdateProject(string projectId, int personId, ProjectData en, ProjectData de, string updatedBy, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(Container container) : IData
{
    public async Task UpdateProject(string projectId, int personId, ProjectData en, ProjectData de, string updatedBy, CancellationToken ct = default)
    {
        // Read first to preserve the current isActive — an edit must not resurrect a soft-deleted project.
        var existing = await container.ReadItemAsync<ProjectDocument>(
            projectId, new PartitionKey(ProjectDocument.PartitionKeyValue), cancellationToken: ct);

        var document = ProjectDocument.FromInput(projectId, personId, en, de, updatedBy, existing.Resource.IsActive);

        await container.ReplaceItemAsync(document, projectId, new PartitionKey(ProjectDocument.PartitionKeyValue), cancellationToken: ct);
    }
}

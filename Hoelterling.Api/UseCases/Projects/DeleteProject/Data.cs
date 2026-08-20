using FastEndpoints;
using Hoelterling.Api.UseCases.Projects.Dtos;
using Microsoft.Azure.Cosmos;

namespace Hoelterling.Api.UseCases.Projects.DeleteProject;

public interface IData
{
    Task DeleteProject(string projectId, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(Container container) : IData
{
    public async Task DeleteProject(string projectId, CancellationToken ct = default)
    {
        // Soft delete (matches the old behaviour). Patch sets a single field without a read + full replace.
        await container.PatchItemAsync<ProjectDocument>(
            projectId,
            new PartitionKey(ProjectDocument.PartitionKeyValue),
            [PatchOperation.Set("/isActive", false)],
            cancellationToken: ct);
    }
}

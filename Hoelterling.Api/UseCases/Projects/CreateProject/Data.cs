using FastEndpoints;
using Hoelterling.Api.UseCases.Projects.Dtos;
using Microsoft.Azure.Cosmos;

namespace Hoelterling.Api.UseCases.Projects.CreateProject;

public interface IData
{
    Task<string> CreateProject(int personId, ProjectData en, ProjectData de, string createdBy, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(Container container) : IData
{
    public async Task<string> CreateProject(int personId, ProjectData en, ProjectData de, string createdBy, CancellationToken ct = default)
    {
        var id = $"project-{Guid.NewGuid():N}";
        var document = ProjectDocument.FromInput(id, personId, en, de, createdBy);

        await container.CreateItemAsync(document, new PartitionKey(ProjectDocument.PartitionKeyValue), cancellationToken: ct);

        return id;
    }
}

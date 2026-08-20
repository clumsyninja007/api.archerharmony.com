using FastEndpoints;
using Hoelterling.Api.UseCases.WorkExperience.Dtos;
using Microsoft.Azure.Cosmos;

namespace Hoelterling.Api.UseCases.WorkExperience.CreateWorkExperience;

public interface IData
{
    Task<string> CreateWorkExperience(int personId, WorkExperienceData en, WorkExperienceData de, string createdBy, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(Container container) : IData
{
    public async Task<string> CreateWorkExperience(int personId, WorkExperienceData en, WorkExperienceData de, string createdBy, CancellationToken ct = default)
    {
        var id = $"we-{Guid.NewGuid():N}";
        var document = WorkExperienceDocument.FromInput(id, personId, en, de, createdBy);

        // One document = one atomic write; the old multi-table transaction is gone.
        await container.CreateItemAsync(document, new PartitionKey(WorkExperienceDocument.PartitionKeyValue), cancellationToken: ct);

        return id;
    }
}

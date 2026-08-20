using FastEndpoints;
using Hoelterling.Api.UseCases.WorkExperience.Dtos;
using Microsoft.Azure.Cosmos;

namespace Hoelterling.Api.UseCases.WorkExperience.DeleteWorkExperience;

public interface IData
{
    Task DeleteWorkExperience(string experienceId, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(Container container) : IData
{
    public async Task DeleteWorkExperience(string experienceId, CancellationToken ct = default)
    {
        // Hard delete (matches the old behaviour for work experience).
        await container.DeleteItemAsync<WorkExperienceDocument>(
            experienceId, new PartitionKey(WorkExperienceDocument.PartitionKeyValue), cancellationToken: ct);
    }
}

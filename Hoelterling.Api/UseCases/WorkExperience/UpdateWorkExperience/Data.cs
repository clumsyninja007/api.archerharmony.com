using FastEndpoints;
using Hoelterling.Api.UseCases.WorkExperience.Dtos;
using Microsoft.Azure.Cosmos;

namespace Hoelterling.Api.UseCases.WorkExperience.UpdateWorkExperience;

public interface IData
{
    Task UpdateWorkExperience(string experienceId, int personId, WorkExperienceData en, WorkExperienceData de, string updatedBy, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(Container container) : IData
{
    public async Task UpdateWorkExperience(string experienceId, int personId, WorkExperienceData en, WorkExperienceData de, string updatedBy, CancellationToken ct = default)
    {
        var document = WorkExperienceDocument.FromInput(experienceId, personId, en, de, updatedBy);

        // Replace the whole document — the embedded skills ARE the document, so no delete-and-reinsert.
        await container.ReplaceItemAsync(document, experienceId, new PartitionKey(WorkExperienceDocument.PartitionKeyValue), cancellationToken: ct);
    }
}

using FastEndpoints;
using Hoelterling.Api.UseCases.PersonalInfo.Dtos;
using Microsoft.Azure.Cosmos;

namespace Hoelterling.Api.UseCases.PersonalInfo.UpdatePersonalInfo;

public interface IData
{
    Task UpdatePersonalInfo(int personId, PersonalInfoData en, PersonalInfoData de, string updatedBy, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(Container container) : IData
{
    public async Task UpdatePersonalInfo(int personId, PersonalInfoData en, PersonalInfoData de, string updatedBy, CancellationToken ct = default)
    {
        // English lives in the base fields, German in localizations.de — so this replaces the whole profile doc.
        var document = ProfileDocument.FromInput(personId, en, de, updatedBy);

        await container.ReplaceItemAsync(
            document, ProfileDocument.BuildId(personId), new PartitionKey(ProfileDocument.PartitionKeyValue), cancellationToken: ct);
    }
}

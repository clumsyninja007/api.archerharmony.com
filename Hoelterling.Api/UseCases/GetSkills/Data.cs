using FastEndpoints;
using Hoelterling.Api.Helpers;
using Microsoft.Azure.Cosmos;

namespace Hoelterling.Api.UseCases.GetSkills;

public interface IData
{
    public Task<List<string>> GetSkills(int personId, string language, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(Container container) : IData
{
    public async Task<List<string>> GetSkills(int personId, string language, CancellationToken ct = default)
    {
        const string query =
            """
            SELECT c.label
                ,c.localizations
            FROM c
            WHERE c.type = "skill"
                AND c.personId = @personId
            """;

        using var iterator = container.GetItemQueryIterator<SkillDoc>(
            new QueryDefinition(query).WithParameter("@personId", personId),
            requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey("skill") });
        
        var results = new List<string>();
        while (iterator.HasMoreResults)
        {
            foreach (var doc in await iterator.ReadNextAsync(ct))
            {
                results.Add(LocalizationHelper.Localize(doc.Label, doc.Localizations, language, "label"));
            }
        }
        return results;
    }
}
using FastEndpoints;
using Hoelterling.Api.Extensions;
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

        var docs = await container.QueryAsync<SkillDoc>(
            new QueryDefinition(query).WithParameter("@personId", personId),
            new PartitionKey("skill"), ct);

        return docs
            .Select(doc => LocalizationHelper.Localize(doc.Label, doc.Localizations, language, "label"))
            .ToList();
    }
}

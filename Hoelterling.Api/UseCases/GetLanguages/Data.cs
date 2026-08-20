using FastEndpoints;
using Hoelterling.Api.Extensions;
using Hoelterling.Api.Helpers;
using Microsoft.Azure.Cosmos;

namespace Hoelterling.Api.UseCases.GetLanguages;

public interface IData
{
    public Task<List<string>> GetLanguages(int personId, string language, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(Container container) : IData
{
    public async Task<List<string>> GetLanguages(int personId, string language, CancellationToken ct = default)
    {
        const string query =
            """
            SELECT c.language
                ,c.proficiencyLevel
                ,c.displayOrder
                ,c.localizations
            FROM c
            WHERE c.type = "personLanguage"
                AND c.personId = @personId
            ORDER BY c.displayOrder
            """;

        var docs = await container.QueryAsync<LanguageDoc>(
            new QueryDefinition(query).WithParameter("@personId", personId),
            new PartitionKey("personLanguage"), ct);

        return docs
            .Select(doc =>
                $"{LocalizationHelper.Localize(doc.Language, doc.Localizations, language, "language")} " +
                $"({LocalizationHelper.Localize(doc.ProficiencyLevel, doc.Localizations, language, "proficiencyLevel")})")
            .ToList();
    }
}

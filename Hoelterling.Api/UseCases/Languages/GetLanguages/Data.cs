using FastEndpoints;
using Hoelterling.Api.Extensions;
using Hoelterling.Api.Helpers;
using Hoelterling.Api.UseCases.Languages.Dtos;
using Microsoft.Azure.Cosmos;

namespace Hoelterling.Api.UseCases.Languages.GetLanguages;

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
                AND c.isActive = true
            ORDER BY c.displayOrder
            """;

        var docs = await container.QueryAsync<LanguageDocument>(
            new QueryDefinition(query).WithParameter("@personId", personId),
            new PartitionKey(LanguageDocument.PartitionKeyValue), ct);

        return docs
            .Select(doc =>
                $"{LocalizationHelper.Localize(doc.Language, doc.Localizations, language, "language")} " +
                $"({LocalizationHelper.Localize(doc.ProficiencyLevel, doc.Localizations, language, "proficiencyLevel")})")
            .ToList();
    }
}

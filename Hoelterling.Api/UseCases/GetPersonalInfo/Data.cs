using FastEndpoints;
using Hoelterling.Api.Extensions;
using Hoelterling.Api.Helpers;
using Microsoft.Azure.Cosmos;

namespace Hoelterling.Api.UseCases.GetPersonalInfo;

public interface IData
{
    Task<PersonWithDescription?> GetPersonalInfo(int personId, string language, CancellationToken ct = default);
    Task<List<ContactInfo>> GetContactInfo(int personId, string language, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(Container container) : IData
{
    public async Task<PersonWithDescription?> GetPersonalInfo(int personId, string language, CancellationToken ct = default)
    {
        const string query =
            """
            SELECT c.name
                ,c.title
                ,c.heroDescription
                ,c.localizations
            FROM c
            WHERE c.type = "profile"
                AND c.personId = @personId
            """;

        var docs = await container.QueryAsync<ProfileDoc>(
            new QueryDefinition(query).WithParameter("@personId", personId),
            new PartitionKey("profile"), ct);

        var profile = docs.FirstOrDefault();
        if (profile is null)
        {
            return null;
        }

        return new PersonWithDescription(
            profile.Name, // NOT localized (proper noun)
            LocalizationHelper.Localize(profile.Title, profile.Localizations, language, "title"),
            profile.HeroDescription is null
                ? null
                : LocalizationHelper.Localize(profile.HeroDescription, profile.Localizations, language, "heroDescription"));
    }

    public async Task<List<ContactInfo>> GetContactInfo(int personId, string language, CancellationToken ct = default)
    {
        const string query =
            """
            SELECT c.label
                ,c.link
                ,c.icon
                ,c.localizations
            FROM c
            WHERE c.type = "contact"
                AND c.personId = @personId
            """;

        var docs = await container.QueryAsync<ContactDoc>(
            new QueryDefinition(query).WithParameter("@personId", personId),
            new PartitionKey("contact"), ct);

        // link & icon are not localized
        return docs
            .Select(doc => new ContactInfo(
                LocalizationHelper.Localize(doc.Label, doc.Localizations, language, "label"),
                doc.Link,
                doc.Icon))
            .ToList();
    }
}

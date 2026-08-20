namespace Hoelterling.Api.UseCases.PersonalInfo.Dtos;

// Canonical Cosmos document for the (singleton) profile. Used by reads (deserialize) and the update (construct).
public record ProfileDocument(
    string Id,
    string Type,
    int PersonId,
    string Name,
    string Title,
    string? HeroDescription,
    Dictionary<string, Dictionary<string, string?>>? Localizations,
    string? UpdatedBy = null)
{
    public const string PartitionKeyValue = "profile";

    // One profile document per person: id is deterministic.
    public static string BuildId(int personId) => $"person-{personId}";

    // Name is a proper noun and not localized; Title/HeroDescription get the German localization.
    // Name/Title are required on update (the admin UI always supplies them).
    public static ProfileDocument FromInput(int personId, PersonalInfoData en, PersonalInfoData de, string updatedBy)
    {
        var localizations = new Dictionary<string, Dictionary<string, string?>>
        {
            ["de"] = new() { ["title"] = de.Title, ["heroDescription"] = de.HeroDescription }
        };

        return new ProfileDocument(
            Id: BuildId(personId),
            Type: PartitionKeyValue,
            PersonId: personId,
            Name: en.Name!,
            Title: en.Title!,
            HeroDescription: en.HeroDescription,
            Localizations: localizations,
            UpdatedBy: updatedBy);
    }
}

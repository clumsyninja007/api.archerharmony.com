namespace Hoelterling.Api.UseCases.PersonalInfo.Dtos;

// Canonical Cosmos document for a contact entry (read-only — no write endpoint touches contacts).
public record ContactDocument(
    string Id,
    string Type,
    int PersonId,
    string Label,
    string? Link,
    string Icon,
    Dictionary<string, Dictionary<string, string?>>? Localizations,
    string? UpdatedBy = null)
{
    public const string PartitionKeyValue = "contact";
}

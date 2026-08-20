namespace Hoelterling.Api.UseCases.Projects.Dtos;

// Canonical Cosmos document for a project. Used by reads (deserialize) and writes (construct).
public record ProjectDocument(
    string Id,
    string Type,
    int PersonId,
    string Title,
    string Description,
    string LongDescription,
    string? ImageUrl,
    string? LiveUrl,
    string? DemoUrl,
    string? GithubUrl,
    int DisplayOrder,
    bool IsActive,
    List<ProjectTechnologyItem> Technologies,
    Dictionary<string, Dictionary<string, string?>>? Localizations,
    string? UpdatedBy = null)
{
    public const string PartitionKeyValue = "project";

    // Assembles a full document from the English base (en) + German localizations (de).
    // URLs / display order are not localized.
    public static ProjectDocument FromInput(
        string id, int personId, ProjectData en, ProjectData de, string updatedBy, bool isActive = true)
    {
        var technologies = en.Technologies
            .Select((tech, i) => new ProjectTechnologyItem(
                tech,
                i,
                i < de.Technologies.Count
                    ? new Dictionary<string, Dictionary<string, string?>> { ["de"] = new() { ["technology"] = de.Technologies[i] } }
                    : null))
            .ToList();

        var localizations = new Dictionary<string, Dictionary<string, string?>>
        {
            ["de"] = new()
            {
                ["title"] = de.Title,
                ["description"] = de.Description,
                ["longDescription"] = de.LongDescription
            }
        };

        return new ProjectDocument(
            Id: id,
            Type: PartitionKeyValue,
            PersonId: personId,
            Title: en.Title,
            Description: en.Description,
            LongDescription: en.LongDescription,
            ImageUrl: en.ImageUrl,
            LiveUrl: en.LiveUrl,
            DemoUrl: en.DemoUrl,
            GithubUrl: en.GithubUrl,
            DisplayOrder: en.DisplayOrder,
            IsActive: isActive,
            Technologies: technologies,
            Localizations: localizations,
            UpdatedBy: updatedBy);
    }
}

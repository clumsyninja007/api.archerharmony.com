namespace Hoelterling.Api.UseCases.WorkExperience.Dtos;

// Canonical Cosmos document for a work experience. Used by reads (deserialize) and writes (construct).
public record WorkExperienceDocument(
    string Id,
    string Type,
    int PersonId,
    string Title,
    string Company,
    string Location,
    DateTime StartDate,
    DateTime? EndDate,
    List<WorkExperienceSkillItem> Skills,
    Dictionary<string, Dictionary<string, string?>>? Localizations,
    string? UpdatedBy = null)
{
    public const string PartitionKeyValue = "workExperience";

    // Assembles a full document from the English base (en) + German localizations (de).
    // Company is intentionally not localized (proper noun).
    public static WorkExperienceDocument FromInput(
        string id, int personId, WorkExperienceData en, WorkExperienceData de, string updatedBy)
    {
        var skills = en.Skills
            .Select((skill, i) => new WorkExperienceSkillItem(
                skill,
                i,
                i < de.Skills.Count
                    ? new Dictionary<string, Dictionary<string, string?>> { ["de"] = new() { ["skill"] = de.Skills[i] } }
                    : null))
            .ToList();

        var localizations = new Dictionary<string, Dictionary<string, string?>>
        {
            ["de"] = new() { ["title"] = de.Title, ["location"] = de.Location }
        };

        return new WorkExperienceDocument(
            Id: id,
            Type: PartitionKeyValue,
            PersonId: personId,
            Title: en.Title,
            Company: en.Company,
            Location: en.Location,
            StartDate: en.StartDate,
            EndDate: en.EndDate,
            Skills: skills,
            Localizations: localizations,
            UpdatedBy: updatedBy);
    }
}

namespace Hoelterling.Api.UseCases.WorkExperience.Dtos;

// Shared request-input shape for Create + Update (one language's worth of fields).
public record WorkExperienceData
{
    public string Title { get; init; } = null!;
    public string Company { get; init; } = null!;
    public string Location { get; init; } = null!;
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public List<string> Skills { get; init; } = [];
}

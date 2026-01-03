namespace api.archerharmony.com.Features.Hoelterling.CreateWorkExperience;

public record Request
{
    public int PersonId { get; init; }
    public WorkExperienceData En { get; init; } = null!;
    public WorkExperienceData De { get; init; } = null!;
}

public record WorkExperienceData
{
    public string Title { get; init; } = null!;
    public string Company { get; init; } = null!;
    public string Location { get; init; } = null!;
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public List<string> Skills { get; init; } = [];
}

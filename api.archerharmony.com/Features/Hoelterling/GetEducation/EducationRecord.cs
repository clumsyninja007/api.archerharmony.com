namespace api.archerharmony.com.Features.Hoelterling.GetEducation;

public record EducationRecord
{
    public required string School { get; init; }
    public string? Degree { get; init; }
    public string? Major { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
}
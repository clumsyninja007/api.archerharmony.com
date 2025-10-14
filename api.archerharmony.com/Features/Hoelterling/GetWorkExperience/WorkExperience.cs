namespace api.archerharmony.com.Features.Hoelterling.GetWorkExperience;

public record WorkExperience(
    string Title,
    string Company,
    string Location,
    DateTime StartDate,
    DateTime? EndDate,
    IEnumerable<string> Skills);
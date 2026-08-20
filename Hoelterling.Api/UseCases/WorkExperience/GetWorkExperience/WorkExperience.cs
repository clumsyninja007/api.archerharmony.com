namespace Hoelterling.Api.UseCases.WorkExperience.GetWorkExperience;

public sealed record WorkExperience(
    string Id,
    string Title,
    string Company,
    string Location,
    DateTime StartDate,
    DateTime? EndDate,
    IEnumerable<string> Skills);

namespace Hoelterling.Api.UseCases.GetWorkExperience;

public sealed record WorkExperience(
    string Id,
    string Title,
    string Company,
    string Location,
    DateTime StartDate,
    DateTime? EndDate,
    IEnumerable<string> Skills);
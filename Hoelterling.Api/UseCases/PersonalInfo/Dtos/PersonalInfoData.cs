namespace Hoelterling.Api.UseCases.PersonalInfo.Dtos;

// Request-input shape for UpdatePersonalInfo (one language's worth of fields).
public record PersonalInfoData
{
    public string? Name { get; init; }
    public string? Title { get; init; }
    public string? HeroDescription { get; init; }
}

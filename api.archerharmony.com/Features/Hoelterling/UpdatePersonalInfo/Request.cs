namespace api.archerharmony.com.Features.Hoelterling.UpdatePersonalInfo;

public record Request
{
    public int PersonId { get; init; }
    public PersonalInfoData En { get; init; } = null!;
    public PersonalInfoData De { get; init; } = null!;
}

public record PersonalInfoData
{
    public string? Name { get; init; }
    public string? Title { get; init; }
    public string? HeroDescription { get; init; }
}
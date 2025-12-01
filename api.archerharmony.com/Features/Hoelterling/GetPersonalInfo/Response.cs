namespace api.archerharmony.com.Features.Hoelterling.GetPersonalInfo;

public record Response : Person
{
    public string? HeroDescription { get; init; }
    public List<ContactInfo>? ContactInfo { get; init; }
}
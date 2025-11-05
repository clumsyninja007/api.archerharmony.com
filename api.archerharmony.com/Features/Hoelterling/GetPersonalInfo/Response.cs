namespace api.archerharmony.com.Features.Hoelterling.GetPersonalInfo;

public record Response : Person
{
    public List<ContactInfo>? ContactInfo { get; init; }
}
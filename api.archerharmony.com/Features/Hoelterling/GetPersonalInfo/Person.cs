namespace api.archerharmony.com.Features.Hoelterling.GetPersonalInfo;

public record Person
{
    public required string Name { get; init; }
    public required string Title { get; init; }
}
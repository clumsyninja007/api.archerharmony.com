namespace Hoelterling.Api.UseCases.PersonalInfo.GetPersonalInfo;

public sealed record Response
{
    public required string Name { get; init; }
    public required string Title { get; init; }
    public string? HeroDescription { get; init; }
    public List<ContactInfo>? ContactInfo { get; init; }
}

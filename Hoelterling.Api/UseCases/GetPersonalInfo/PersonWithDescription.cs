namespace Hoelterling.Api.UseCases.GetPersonalInfo;

// Localized profile fields, assembled with the contact list into the Response by the endpoint.
public sealed record PersonWithDescription(string Name, string Title, string? HeroDescription);

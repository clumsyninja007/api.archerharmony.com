using Hoelterling.Api.UseCases.PersonalInfo.Dtos;

namespace Hoelterling.Api.UseCases.PersonalInfo.UpdatePersonalInfo;

public sealed record Request
{
    public int PersonId { get; init; }
    public PersonalInfoData En { get; init; } = null!;
    public PersonalInfoData De { get; init; } = null!;
}

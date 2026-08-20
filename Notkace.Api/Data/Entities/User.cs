namespace Notkace.Api.Data.Entities;

public class User
{
    public long Id { get; set; }
    public string? UserName { get; set; }
    public string? FullName { get; set; }
    public long? RoleId { get; set; }
}

namespace Notkace.Api.Data.Entities;

public class Asset
{
    public long Id { get; set; }
    public long AssetTypeId { get; set; }
    public string Name { get; set; } = null!;
}

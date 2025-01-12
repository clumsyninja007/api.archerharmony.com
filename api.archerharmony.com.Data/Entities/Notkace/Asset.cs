namespace api.archerharmony.com.Entities.Entities.Notkace;

public partial class Asset
{
    public ulong Id { get; set; }

    public ulong AssetTypeId { get; set; }

    public string Name { get; set; } = null!;

    public ulong? AssetDataId { get; set; }

    public ulong? OwnerId { get; set; }

    public ulong? LocationId { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? Created { get; set; }

    public ulong? MappedId { get; set; }

    public ulong? AssetClassId { get; set; }

    public string? Archive { get; set; }

    public DateTime? ArchiveDate { get; set; }

    public string? ArchiveReason { get; set; }

    public ulong? AssetStatusId { get; set; }

    public virtual ICollection<HdTicket> HdTicketAssets { get; set; } = new List<HdTicket>();

    public virtual ICollection<HdTicket> HdTicketMachines { get; set; } = new List<HdTicket>();
}

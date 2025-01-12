namespace api.archerharmony.com.Features.Notkace.Tickets.GetTicketInfo;

public record Response
{
    public ulong Ticket { get; init; }
    public string? Summary { get; init; }
    public string? Comment { get; init; }
    public DateTime Timestamp { get; init; }
    public string? Commenter { get; init; }
    public bool OwnersOnly { get; init; }
}
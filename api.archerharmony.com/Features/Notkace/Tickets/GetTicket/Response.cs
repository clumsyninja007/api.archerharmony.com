namespace api.archerharmony.com.Features.Notkace.Tickets.GetTicket;

public record Response
{
    public ulong Ticket { get; init; }
    public required string Title { get; init; }
    public required string Priority { get; init; }
    public required string Owner { get; init; }
    public required string Submitter { get; init; }
    public required string Asset { get; init; }
    public string? Device { get; init; }
    public required string Status { get; init; }
    public string? ReferredTo { get; init; }
    public required string UserName { get; init; }
    public required string Dept { get; init; }
    public required string Location { get; init; }
    public ulong PriOrd { get; init; }
    public ulong StatOrd { get; init; }
    public DateTimeOffset Created { get; init; }
}
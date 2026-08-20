namespace Notkace.Api.UseCases.Tickets.GetTicket;

public record Response
{
    public long Ticket { get; init; }
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
    public long PriOrd { get; init; }
    public long StatOrd { get; init; }
    public DateTimeOffset Created { get; init; }
}

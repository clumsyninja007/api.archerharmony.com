namespace api.archerharmony.com.Features.Notkace.Tickets.GetTickets;

public record Request
{
    public string? Assignee { get; init; }
    public string? Software { get; init; }
    public string? ReferredTo { get; init; }
    public string? Department { get; init; }
    public string? Location { get; init; }
    public string? ExcludedStatuses { get; init; }
    public string? Sort { get; init; }
    public int Page { get; init; } = 1;
    public int PerPage { get; init; } = 20;
}
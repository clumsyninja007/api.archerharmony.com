namespace api.archerharmony.com.Features.Notkace.Tickets.GetTickets;

public record Request
{
    [FromQueryParams]
    public string? Assignee { get; init; }
    [FromQueryParams]
    public string? Software { get; init; }
    [FromQueryParams]
    public string? ReferredTo { get; init; }
    [FromQueryParams]
    public string? Department { get; init; }
    [FromQueryParams]
    public string? Location { get; init; }
    [FromQueryParams]
    public string? ExcludedStatuses { get; init; }
    [FromQueryParams]
    public string? Sort { get; init; }
    [FromQueryParams]
    public int Page { get; init; } = 1;
    [FromQueryParams]
    public int PerPage { get; init; } = 20;
}
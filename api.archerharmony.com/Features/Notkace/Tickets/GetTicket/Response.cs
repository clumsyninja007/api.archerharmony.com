using System;

namespace api.archerharmony.com.Features.Notkace.Tickets.GetTicket;

public record Response
{
    public long Ticket { get; init; }
    public string Title { get; init; }
    public string Priority { get; init; }
    public string Owner { get; init; }
    public string Submitter { get; init; }
    public string Asset { get; init; }
    public string Device { get; init; }
    public string Status { get; init; }
    public string ReferredTo { get; init; }
    public string UserName { get; init; }
    public string Dept { get; init; }
    public string Location { get; init; }
    public long PriOrd { get; init; }
    public long StatOrd { get; init; }
    public DateTimeOffset Created { get; init; }
}
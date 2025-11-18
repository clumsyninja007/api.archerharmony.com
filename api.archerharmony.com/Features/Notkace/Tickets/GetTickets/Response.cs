using api.archerharmony.com.Extensions;

namespace api.archerharmony.com.Features.Notkace.Tickets.GetTickets;

public class Response
{
    public required PaginatedList<GetTicket.Response> Result { get; init; }
    public int Total => Result.TotalRows;
    public int Count => Result.Count;
}
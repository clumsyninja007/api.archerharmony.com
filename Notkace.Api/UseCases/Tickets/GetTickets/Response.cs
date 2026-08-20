using Notkace.Api.Extensions;

namespace Notkace.Api.UseCases.Tickets.GetTickets;

public class Response
{
    public required PaginatedList<GetTicket.Response> Result { get; init; }
    public int Total => Result.TotalRows;
    public int Count => Result.Count;
}

using api.archerharmony.com.Extensions;

namespace api.archerharmony.com.Classes.Notkace
{
    public class HdTicketsOutputDto
    {
        public PaginatedList<HdTicketsDto> Result { get; set; }
        public int Total => Result.TotalRows;
        public int Count => Result.Count;
    }
}

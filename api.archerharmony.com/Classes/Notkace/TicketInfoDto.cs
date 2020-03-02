using System;

namespace api.archerharmony.com.Classes.Notkace
{
    public class TicketInfoDto
    {
        public long Ticket { get; set; }
        public string Summary { get; set; }
        public string Comment { get; set; }
        public DateTime Timestamp { get; set; }
        public string Commenter { get; set; }
        public byte OwnersOnly { get; set; }
    }
}

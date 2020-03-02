using System;

namespace api.archerharmony.com.Classes.Notkace
{
    public class HdTicketsDto
    {
        public long Ticket { get; set; }
        public string Title { get; set; }
        public string Priority { get; set; }
        public string Owner { get; set; }
        public string Submitter { get; set; }
        public string Asset { get; set; }
        public string Device { get; set; }
        public string Status { get; set; }
        public string ReferredTo { get; set; }
        public string UserName { get; set; }
        public string Dept { get; set; }
        public string Location { get; set; }
        public long PriOrd { get; set; }
        public long StatOrd { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}

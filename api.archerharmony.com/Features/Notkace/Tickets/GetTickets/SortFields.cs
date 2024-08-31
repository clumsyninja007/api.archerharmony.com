namespace api.archerharmony.com.Features.Notkace.Tickets.GetTickets;

public class SortFields
{
    public SortFields(string sort)
    {
        Fields = [];
        
        var sortArray = sort.Split(',');
        foreach (var sortItem in sortArray)
        {
            Fields.Add(new SortField(sortItem));
        }
    }

    public List<SortField> Fields { get; }

    public bool Contains(string field)
    {
        return Fields.Any(f => f.Field == field);
    }
}

public class SortField(string field)
{
    public string Field { get; } = field[1..].ToLower();
    public bool SortDesc { get; } = field[0] == '-';
}
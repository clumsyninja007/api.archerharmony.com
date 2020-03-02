using System.Collections.Generic;
using System.Linq;

namespace api.archerharmony.com.Classes.Notkace
{
    public class SortFields
    {
        public SortFields(string sort)
        {
            string[] sortArray = sort?.Split(',');
            if (sortArray == null)
            {
                Fields = null;
            }
            else
            {
                Fields = new List<SortField>();

                foreach (var sortItem in sortArray) Fields.Add(new SortField(sortItem));
            }
        }

        public List<SortField> Fields { get; }

        public bool Contains(string field)
        {
            return Fields != null && Fields.Any(f => f.Field == field);
        }
    }

    public class SortField
    {
        public SortField(string field)
        {
            Field = field.Substring(1).ToLower();
            SortDesc = field[0] == '-';
        }

        public string Field { get; }
        public bool SortDesc { get; }
    }
}

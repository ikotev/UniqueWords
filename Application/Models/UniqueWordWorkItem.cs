using System.Collections.Generic;

namespace UniqueWords.Application.Models
{
    public class UniqueWordWorkItem : List<string>
    {
        public UniqueWordWorkItem(IEnumerable<string> collection)
        : base(collection)
        { }
    }
}
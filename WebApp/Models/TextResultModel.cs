using System.Collections.Generic;

namespace UniqueWords.WebApp.Models
{    
    public class TextResultModel
    {
        public int DistinctWords { get; set; }

        public int DistinctUniqueWords { get; set; }

        public List<string> WatchlistWords { get; set; }
    }
}

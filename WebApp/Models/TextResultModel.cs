namespace UniqueWords.WebApp.Models
{
    using System.Collections.Generic;

    public class TextResultModel
    {
        public int DistinctUniqueWords { get; set; }

        public List<string> WatchlistWords { get; set; }
    }
}

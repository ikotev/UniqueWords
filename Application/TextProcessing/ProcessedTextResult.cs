using System.Collections.Generic;

namespace UniqueWords.Application.TextProcessing
{    
    public class ProcessedTextResult
    {
        public int DistinctWords { get; set; }

        public int DistinctUniqueWords { get; set; }

        public List<string> WatchlistWords { get; set; }
    }
}
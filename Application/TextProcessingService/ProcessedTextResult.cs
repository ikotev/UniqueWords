namespace UniqueWords.Application.TextProcessing
{
    using System.Collections.Generic;

    public class ProcessedTextResult
    {
        public int DistinctWords { get; set; }

        public int DistinctUniqueWords { get; set; }

        public List<string> WatchlistWords { get; set; }
    }
}
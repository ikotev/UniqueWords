using System.Collections.Generic;

namespace UniqueWords.Application.WordsAdding
{
    public class WordsWorkQueueItem
    {
        public List<string> Words { get; private set; }

        public WordsWorkQueueItem(IEnumerable<string> words)
        { 
            Words = new List<string>(words);
        }
    }
}
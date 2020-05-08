using System.Collections.Generic;

namespace UniqueWords.Application.UniqueWordsWorkItem
{
    public class UniqueWordsMessage
    {
        public List<string> Words { get; private set; }

        public UniqueWordsMessage(IEnumerable<string> words)
        { 
            Words = new List<string>(words);
        }
    }
}
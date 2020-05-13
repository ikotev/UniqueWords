using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniqueWords.Application.WordsAdding;
using UniqueWords.Application.WorkQueue;

namespace UniqueWords.Application.TextProcessing
{

    public class UniqueWordsAddingBackendSync : IUniqueWordsAddingStrategy
    {
        private readonly IWorkQueuePublisher<WordsWorkQueueItem> _workQueuePublisher;

        public UniqueWordsAddingBackendSync(IWorkQueuePublisher<WordsWorkQueueItem> workQueuePublisher)
        {
            _workQueuePublisher = workQueuePublisher;
        }

        public async Task<List<string>> AddUniqueWordsAsync(IWordsRepository wordsRepository, List<string> words)
        {
            var uniqueWords = await FindUniqueWordsAsync(wordsRepository, words);

            if (uniqueWords.Any())
            {
                _workQueuePublisher.Publish(new WordsWorkQueueItem(uniqueWords));
            }

            return uniqueWords;
        }

        private async Task<List<string>> FindUniqueWordsAsync(IWordsRepository wordsRepository, List<string> words)
        {
            var skip = 0;
            var take = 100;
            var n = words.Count;
            var uniqueWords = new List<string>();

            while (skip < n)
            {
                var wordsPart = words
                .Skip(skip)
                .Take(take)
                .ToList();

                var foundWords = await wordsRepository.FindAsync(wordsPart);
                var uniqueWordsPart = wordsPart
                .Except(foundWords.Select(wi => wi.Word));

                uniqueWords.AddRange(uniqueWordsPart);

                skip += take;
            }

            return uniqueWords;
        }
    }
}

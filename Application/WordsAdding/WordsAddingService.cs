using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniqueWords.Application.TextProcessing;
using UniqueWords.Application.WorkQueue;

namespace UniqueWords.Application.WordsAdding
{
    public class WordsAddingService : IWorkItemHandler<WordsWorkQueueItem>
    {
        private readonly IWordsAddingDataContextFactory _dataContextFactory;

        public WordsAddingService(IWordsAddingDataContextFactory dataContextFactory)
        {
            _dataContextFactory = dataContextFactory;
        }

        public async Task HandleAsync(WordsWorkQueueItem message)
        {            
            using (var db = _dataContextFactory.Create())
            {
                  await AddNewWordsAsync(db.WordsRepository, message.Words);              
            }            
        }

        private async Task AddNewWordsAsync(IWordsRepository wordsRepository, List<string> words)
        {
            var skip = 0;
            var take = 100;
            var n = words.Count;            

            while (skip < n)
            {
                var wordsPart = words
                .Skip(skip)
                .Take(take)
                .ToList();

                await wordsRepository.TryAddNewWordsWithNoSyncAsync(wordsPart);                   

                skip += take;
            }
        }
    }
}
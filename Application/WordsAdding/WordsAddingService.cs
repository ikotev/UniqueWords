using System.Threading.Tasks;
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

        public Task HandleAsync(WordsWorkQueueItem message)
        {            
            using (var db = _dataContextFactory.Create())
            {
                
            }

            return Task.CompletedTask;
        }
    }
}
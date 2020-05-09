using System.Threading.Tasks;
using UniqueWords.Application.WorkQueue;

namespace UniqueWords.Application.WordsWorkQueue
{
    public class WordsWorkQueueItemHandler : IWorkItemHandler<WordsWorkQueueItem>
    {
        public Task HandleAsync(WordsWorkQueueItem message)
        {
            System.Console.WriteLine($"number of new words: {message.Words.Count}");

            return Task.CompletedTask;
        }
    }
}
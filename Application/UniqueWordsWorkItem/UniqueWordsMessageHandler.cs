using System.Threading.Tasks;
using UniqueWords.Application.WorkQueue;

namespace UniqueWords.Application.UniqueWordsWorkItem
{
    public class UniqueWordsMessageHandler : IWorkItemHandler<UniqueWordsMessage>
    {
        public Task HandleAsync(UniqueWordsMessage message)
        {
            System.Console.WriteLine(message.Words.Count);

            return Task.CompletedTask;
        }
    }
}
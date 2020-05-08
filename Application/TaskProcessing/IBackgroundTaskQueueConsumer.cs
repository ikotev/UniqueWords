using System.Threading;
using System.Threading.Tasks;

namespace UniqueWords.Application.TaskProcessing
{
    public interface IBackgroundTaskQueueConsumer<T>
    {
        Task<T> ConsumeAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
using System.Threading;
using System.Threading.Tasks;

namespace UniqueWords.Application.WorkQueue
{
    public interface IBackgroundWorkQueueConsumer<T>
    {
        Task<T> ConsumeAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
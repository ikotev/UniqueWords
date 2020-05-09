using System.Threading;
using System.Threading.Tasks;

namespace UniqueWords.Application.WorkQueue
{
    public interface IWorkQueueConsumer<T>
    {
        Task<T> ConsumeAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
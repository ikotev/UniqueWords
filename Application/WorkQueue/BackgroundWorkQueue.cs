using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace UniqueWords.Application.WorkQueue
{    
    public class BackgroundWorkQueue<T> : IBackgroundWorkQueuePublisher<T>, IBackgroundWorkQueueConsumer<T>
    {
        private readonly ConcurrentQueue<T> _workItems;

        private readonly SemaphoreSlim _signal;

        public BackgroundWorkQueue()
        {
            _workItems = new ConcurrentQueue<T>();
            _signal = new SemaphoreSlim(0);
        }

        public void Publish(T workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            QueueBackgroundWorkItem(workItem);
        }

        public Task<T> ConsumeAsync(CancellationToken cancellationToken)
        {
            return DequeueAsync(cancellationToken);
        }

        private void QueueBackgroundWorkItem(T workItem)
        {
            _workItems.Enqueue(workItem);
            _signal.Release();
        }

        private async Task<T> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _workItems.TryDequeue(out var workItem);

            return workItem;
        }
    }
}
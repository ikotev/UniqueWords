using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using UniqueWords.Application.WorkQueue;

namespace WebApp.BackgroundServices
{
    public class WorkQueueConsumerBackgroundService<T> : BackgroundService
    {
        private readonly IBackgroundWorkQueueConsumer<T> _workQueueConsumer;
        private readonly IEnumerable<IWorkItemHandler<T>> _workItemHndlers;

        public WorkQueueConsumerBackgroundService(
            IBackgroundWorkQueueConsumer<T> workQueueConsumer,
            IEnumerable<IWorkItemHandler<T>> workItemHndlers)
        {
            _workQueueConsumer = workQueueConsumer;
            _workItemHndlers = workItemHndlers;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var workItem = await _workQueueConsumer.ConsumeAsync(cancellationToken);

                foreach (var handler in _workItemHndlers)
                {
                    await handler.HandleAsync(workItem);
                }
            }
        }
    }
}
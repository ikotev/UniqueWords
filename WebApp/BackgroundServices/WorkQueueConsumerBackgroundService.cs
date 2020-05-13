using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using UniqueWords.Application.WorkQueue;

namespace UniqueWords.WebApp.BackgroundServices
{
    public class WorkQueueConsumerBackgroundService<T> : BackgroundService
    {
        private readonly IWorkQueueConsumer<T> _workQueueConsumer;
        private readonly IEnumerable<IWorkItemHandler<T>> _workItemHandlers;

        public WorkQueueConsumerBackgroundService(
            IWorkQueueConsumer<T> workQueueConsumer,
            IEnumerable<IWorkItemHandler<T>> workItemHandlers)
        {
            _workQueueConsumer = workQueueConsumer;
            _workItemHandlers = workItemHandlers;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var workItem = await _workQueueConsumer.ConsumeAsync(cancellationToken);
                var handlerTasks = _workItemHandlers.Select(h => h.HandleAsync(workItem));
                await Task.WhenAll(handlerTasks);                
            }
        }
    }
}
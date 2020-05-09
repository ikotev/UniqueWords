using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using UniqueWords.Application.StartupConfigs;

namespace UniqueWords.WebApp.BackgroundServices
{
    public class StartupTasksBackgroundService : BackgroundService
    {
        private readonly IEnumerable<IStartupTask> _startupTasks;

        public StartupTasksBackgroundService(IEnumerable<IStartupTask> startupTasks)
        {
            _startupTasks = startupTasks;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var tasks = _startupTasks.Select(t => t.StartAsync(cancellationToken));
            await Task.WhenAll(tasks);            
        }
    }
}
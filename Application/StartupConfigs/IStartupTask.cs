using System.Threading;
using System.Threading.Tasks;

namespace UniqueWords.Application.StartupConfigs
{
    public interface IStartupTask
    {
        Task StartAsync(CancellationToken cancellationToken);

        Task StopAsync(CancellationToken cancellationToken);
    }
}
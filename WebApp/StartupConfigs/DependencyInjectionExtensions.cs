using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UniqueWords.Application.WordsWorkQueue;
using UniqueWords.WebApp.BackgroundServices;

namespace UniqueWords.WebApp.StartupConfigs
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddSingleton<IHostedService, StartupTasksBackgroundService>()           
                .AddSingleton<IHostedService, WorkQueueConsumerBackgroundService<WordsWorkQueueItem>>();            

            return services;
        }
    }
}

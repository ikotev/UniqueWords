using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UniqueWords.Application.UniqueWordsWorkItem;
using WebApp.BackgroundServices;

namespace UniqueWords.WebApp.StartupConfigs
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHostedService, WorkQueueConsumerBackgroundService<UniqueWordsMessage>>();            

            return services;
        }
    }
}

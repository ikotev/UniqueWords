using Microsoft.Extensions.DependencyInjection;

namespace UniqueWords.WebApp.StartupConfigs
{
    public static class AppLifetimeEventsExtensions
    {
        public static IServiceCollection AddAppLifetimeEventsMonitor(this IServiceCollection services)
        {
            services.AddHostedService<AppLifetimeEventsHostedService>();

            return services;
        }
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UniqueWords.WebApp.StartupConfigs
{
    public static class HealthCheckExtensions
    {
        public static IServiceCollection AddHealthChecksServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("UniqueWordsDbConnection");
            //var applicationInsightsKey = configuration["ApplicationInsightsInstrumentationKey"];

            // services.Configure<HealthCheckPublisherOptions>(
            //     publisherOptions =>
            //     {
            //         publisherOptions.Delay = TimeSpan.FromSeconds(30);
            //         publisherOptions.Period = TimeSpan.FromHours(1);
            //     });

            services.AddHealthChecks()                
                .AddSqlServer(connectionString, "SELECT 1;", "SQL DB");
            //.AddApplicationInsightsPublisher(applicationInsightsKey); // nuget: AspNetCore.HealthChecks.Publisher.ApplicationInsights

            return services;
        }
    }    
}
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace UniqueWords.WebApp.StartupConfigs
{
    public static class CorsServiceExtensions
    {
        public static IServiceCollection AddCorsServices(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .DisallowCredentials();
                });
            });

            return services;
        }

        public static IApplicationBuilder UseCorsServices(this IApplicationBuilder app)
        {
            app.UseCors();

            return app;
        }
    }
}

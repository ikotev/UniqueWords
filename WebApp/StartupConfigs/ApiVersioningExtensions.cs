using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace UniqueWords.WebApp.StartupConfigs
{
    public static class ApiVersioningExtensions
    {
        public static IServiceCollection AddApiVersioningServices(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            services.AddVersionedApiExplorer(options =>
            {
                // options.DefaultApiVersion = ApiVersion.Parse(WebApiDefaults.LatestVersion);
                // options.AssumeDefaultVersionWhenUnspecified = true;                                            
                options.SubstituteApiVersionInUrl = true;
                options.GroupNameFormat = "'v'VVV";
            });

            return services;
        }
    }
}
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;

namespace UniqueWords.WebApp.StartupConfigs
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            //var appName = Assembly.GetExecutingAssembly().GetName().Name;

            services
                .AddOptions<SwaggerGenOptions>()
                .Configure<IApiVersionDescriptionProvider>((options, provider) =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        var info = CreateInfoForApiVersion(description);
                        options.SwaggerDoc(description.GroupName, info);
                    }
                });

            services.AddSwaggerGen(options =>
            {
                options.IncludeXmlComments(XmlCommentsFilePath);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerServices(this IApplicationBuilder app)
        {            
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"{WebApiDefaults.Name} Web API {description.ApiVersion.ToString()}");
                }
            });

            return app;
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {            
            var info = new OpenApiInfo
            {
                Title = $"{WebApiDefaults.Name} Web API",
                // Description = $"{WebApiDefaults.Name} API Description",
                Version = description.ApiVersion.ToString(),
                // TermsOfService = new Uri("https://mywebapi.com"),
                // Contact = new OpenApiContact
                // {
                //     Name = $"{WebApiDefaults.Name} Web API",
                //     Email = "info@webapi.com",
                //     Url = new Uri("https://mywebapi.com")
                // },
                // License = new OpenApiLicense
                // {
                //     Name = "",
                //     Url = new Uri("https://mywebapi.com")
                // }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }

        private static string XmlCommentsFilePath
        {
            get
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                return xmlPath;
            }
        }
    }
}

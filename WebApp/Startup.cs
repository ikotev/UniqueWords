﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UniqueWords.Application.Extensions.DependencyInjection;
using UniqueWords.Infrastructure.Extensions.DependencyInjection;
using UniqueWords.WebApp.StartupConfigs;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace UniqueWords.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();
            services.AddInfrastructure(Configuration);

            services.AddCorsServices();

            services.AddControllers(options =>
            {
                // options.Filters.Add(new ProducesAttribute(MediaTypeNames.Application.Json));
                // options.Filters.Add(new ConsumesAttribute(MediaTypeNames.Application.Json));
            });                

            services.AddDependencies(Configuration);

            services.AddApiVersioningServices();
            services.AddHealthChecksServices(Configuration);
            services.AddSwaggerServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseExceptionHandlerMiddleware();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCorsServices();                        

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerServices();                        
        }
    }
}

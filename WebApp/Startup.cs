using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDependencies(Configuration);

            services.AddSwaggerServices();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCorsServices();

            //app.UseHttpsRedirection();

            app.UseSwaggerServices();

            app.UseExceptionHandlerMiddleware();

            app.UseMvc();
        }
    }
}

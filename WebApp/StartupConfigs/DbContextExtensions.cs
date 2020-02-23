namespace UniqueWords.WebApp.StartupConfigs
{
    using Application;
    using Application.Interfaces;
    using Infrastructure.Persistence;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DbContextExtensions
    {
        public static IServiceCollection AddDbServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UniqueWordsDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("UniqueWordsDbConnection"),
                    b => b.MigrationsAssembly(typeof(UniqueWordsDbContext).Assembly.FullName)));

            services.AddScoped<IUniqueWordsDbContext>(provider => provider.GetService<UniqueWordsDbContext>());

            return services;
        }
    }
}

namespace UniqueWords.WebApp.StartupConfigs
{
    using Application.Interfaces;

    using Infrastructure.Persistence;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DbContextExtensions
    {
        public static IServiceCollection AddDbServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("UniqueWordsDbConnection");

            services.AddDbContext<UniqueWordsDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(UniqueWordsDbContext).Assembly.FullName)));

            services.AddScoped<IUniqueWordsDbContext>(provider => provider.GetService<UniqueWordsDbContext>());

            services.AddSingleton<IUniqueWordsDbContextFactory>(provider => new UniqueWordsDbContextFactory(connectionString));

            return services;
        }
    }
}

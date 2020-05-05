using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniqueWords.Application.Words;
using UniqueWords.Infrastructure.Persistence;
using UniqueWords.Infrastructure.Words;

namespace UniqueWords.Infrastructure
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("UniqueWordsDbConnection");

            services.AddDbContext<UniqueWordsDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(UniqueWordsDbContext).Assembly.FullName)));

            services.AddScoped<UniqueWords.Infrastructure.Persistence.IDbContextFactory<UniqueWordsDbContext>, UniqueWordsDbContextFactory>();
            services.AddScoped<IWordsDataContextFactory, WordsDataContextFactory>();

            return services;
        }        
    }
}
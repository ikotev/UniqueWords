using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniqueWords.Application.TextProcessing;
using UniqueWords.Infrastructure.Persistence;
using UniqueWords.Infrastructure.TextProcessing;

namespace UniqueWords.Infrastructure.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("UniqueWordsDbConnection");

            services.AddDbContext<UniqueWordsDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(UniqueWordsDbContext).Assembly.FullName)));

            services.AddScoped<UniqueWords.Infrastructure.Persistence.IDbContextFactory<UniqueWordsDbContext>, UniqueWordsDbContextFactory>();
            services.AddScoped<ITextProcessingDataContextFactory, TextProcessingDataContextFactory>();

            return services;
        }        
    }
}
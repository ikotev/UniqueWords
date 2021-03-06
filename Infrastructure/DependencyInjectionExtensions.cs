using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniqueWords.Application.TextProcessing;
using UniqueWords.Application.WordsAdding;
using UniqueWords.Infrastructure.Persistence;
using UniqueWords.Infrastructure.TextProcessing;
using UniqueWords.Infrastructure.WordsAdding;

namespace UniqueWords.Infrastructure.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("UniqueWordsDbConnection");

            services.AddDbContext<UniqueWordsDbContext>(
                options => options.UseSqlServer(connectionString, sqlServreOptions =>
                    sqlServreOptions.MigrationsAssembly(typeof(UniqueWordsDbContext).Assembly.FullName)),
                ServiceLifetime.Scoped,
                ServiceLifetime.Singleton)

                .AddSingleton<IDbContextFactory<UniqueWordsDbContext>, UniqueWordsDbContextFactory>()
                .AddSingleton<ITextProcessingDataContextFactory, TextProcessingDataContextFactory>()
                .AddSingleton<IWordsAddingDataContextFactory, WordsAddingDataContextFactory>();

            return services;
        }
    }
}
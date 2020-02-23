namespace UniqueWords.WebApp.StartupConfigs
{
    using Application.Interfaces;
    using Application.UniqueWords;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddSingleton<ITextAnalyzer, SimpleTextAnalyzer>();
            services.AddScoped<IUniqueWordsService, UniqueWordsService>();

            return services;
        }
    }
}

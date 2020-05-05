namespace UniqueWords.Application
{    
    using Microsoft.Extensions.DependencyInjection;
    using UniqueWords.Application.Words;
    using UniqueWords.Application.Words.TextAnalyzers;

    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<ITextAnalyzer, SimpleTextAnalyzer>();
            services.AddScoped<ITextProcessingService, TextProcessingService>();

            return services;
        }
    }
}

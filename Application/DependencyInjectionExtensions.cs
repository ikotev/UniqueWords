namespace UniqueWords.Application
{    
    using Microsoft.Extensions.DependencyInjection;
    using UniqueWords.Application.TextProcessing;
    using UniqueWords.Application.TextProcessing.TextAnalyzers;

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

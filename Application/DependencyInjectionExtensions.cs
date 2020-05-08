using Microsoft.Extensions.DependencyInjection;
using UniqueWords.Application.Models;
using UniqueWords.Application.TextProcessing;
using UniqueWords.Application.TextProcessing.TextAnalyzers;
using UniqueWords.Application.WorkQueue;

namespace UniqueWords.Application.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<ITextAnalyzer, SimpleTextAnalyzer>();
            services.AddScoped<ITextProcessingService, TextProcessingService>();

            services.AddSingleton<BackgroundWorkQueue<UniqueWordWorkItem>>();
            services.AddSingleton<IBackgroundWorkQueuePublisher<UniqueWordWorkItem>>(provider => provider.GetService<BackgroundWorkQueue<UniqueWordWorkItem>>());
            services.AddSingleton<IBackgroundWorkQueueConsumer<UniqueWordWorkItem>>(provider => provider.GetService<BackgroundWorkQueue<UniqueWordWorkItem>>());
            return services;
        }
    }
}

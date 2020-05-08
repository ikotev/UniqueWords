using Microsoft.Extensions.DependencyInjection;
using UniqueWords.Application.TextProcessing;
using UniqueWords.Application.TextProcessing.TextAnalyzers;
using UniqueWords.Application.UniqueWordsWorkItem;
using UniqueWords.Application.WorkQueue;

namespace UniqueWords.Application.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<ITextAnalyzer, SimpleTextAnalyzer>();
            services.AddScoped<ITextProcessingService, TextProcessingService>();

            services.AddSingleton<BackgroundWorkQueue<UniqueWordsMessage>>();
            services.AddSingleton<IBackgroundWorkQueuePublisher<UniqueWordsMessage>>(provider =>
                provider.GetService<BackgroundWorkQueue<UniqueWordsMessage>>());
            services.AddSingleton<IBackgroundWorkQueueConsumer<UniqueWordsMessage>>(provider =>
                provider.GetService<BackgroundWorkQueue<UniqueWordsMessage>>());

            services.AddSingleton<IWorkItemHandler<UniqueWordsMessage>, UniqueWordsMessageHandler>();

            return services;
        }
    }
}

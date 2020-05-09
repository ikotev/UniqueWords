using Microsoft.Extensions.DependencyInjection;
using UniqueWords.Application.StartupConfigs;
using UniqueWords.Application.TextProcessing;
using UniqueWords.Application.TextProcessing.TextAnalyzers;
using UniqueWords.Application.WordsWorkQueue;
using UniqueWords.Application.WorkQueue;

namespace UniqueWords.Application.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services
                .AddSingleton<IServiceResolver, ServiceResolver>()

                .AddSingleton<ITextAnalyzer, SimpleTextAnalyzer>()
                .AddSingleton<IUniqueWordsAddingStrategy, UniqueWordsAddingDbSync>()
                .AddSingleton<IUniqueWordsAddingStrategy, UniqueWordsAddingBackendSync>()
                .AddSingleton<ITextProcessingServiceFactory, TextProcessingServiceFactory>()

                .AddSingleton<WorkQueue<WordsWorkQueueItem>>()
                .AddSingleton<IWorkQueuePublisher<WordsWorkQueueItem>>(provider =>
                    provider.GetService<WorkQueue<WordsWorkQueueItem>>())
                .AddSingleton<IWorkQueueConsumer<WordsWorkQueueItem>>(provider =>
                    provider.GetService<WorkQueue<WordsWorkQueueItem>>())

                .AddSingleton<IWorkItemHandler<WordsWorkQueueItem>, WordsWorkQueueItemHandler>();

            return services;
        }
    }
}

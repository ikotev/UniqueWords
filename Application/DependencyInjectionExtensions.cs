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
            services.AddSingleton<IServiceResolver, ServiceResolver>();
            
            services.AddSingleton<ITextAnalyzer, SimpleTextAnalyzer>();
            services.AddSingleton<IUniqueWordsAddingStrategy, UniqueWordsAddingDbSync>();
            services.AddSingleton<IUniqueWordsAddingStrategy, UniqueWordsAddingBackendSync>();                        

            services.AddSingleton<WorkQueue<WordsWorkQueueItem>>();
            services.AddSingleton<IWorkQueuePublisher<WordsWorkQueueItem>>(provider =>
                provider.GetService<WorkQueue<WordsWorkQueueItem>>());
            services.AddSingleton<IWorkQueueConsumer<WordsWorkQueueItem>>(provider =>
                provider.GetService<WorkQueue<WordsWorkQueueItem>>());

            services.AddSingleton<IWorkItemHandler<WordsWorkQueueItem>, WordsWorkQueueItemHandler>();

            return services;
        }
    }
}

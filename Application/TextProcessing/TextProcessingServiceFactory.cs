using Microsoft.Extensions.Logging;
using UniqueWords.Application.StartupConfigs;
using UniqueWords.Application.TextProcessing.TextAnalyzers;

namespace UniqueWords.Application.TextProcessing
{
    public class TextProcessingServiceFactory : ITextProcessingServiceFactory
    {
        private readonly IServiceResolver _resolver;

        public TextProcessingServiceFactory(IServiceResolver resolver)
        {
            _resolver = resolver;
        }

        public ITextProcessingService Create<TAddingStrategy>() where TAddingStrategy : IUniqueWordsAddingStrategy
        {
            return new TextProcessingService(
                _resolver.GetService<ITextProcessingDataContextFactory>(),
                _resolver.GetService<IUniqueWordsAddingStrategy, TAddingStrategy>(),
                _resolver.GetService<ITextAnalyzer>(),
                _resolver.GetService<ILogger<TextProcessingService>>()
            );
        }
    }
}
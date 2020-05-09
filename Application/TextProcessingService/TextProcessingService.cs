using UniqueWords.Application.TextProcessing.TextAnalyzers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace UniqueWords.Application.TextProcessing
{
    public class TextProcessingService : BaseTextProcessingService<TextProcessingService>
    {
        public TextProcessingService(
            ITextProcessingDataContextFactory dataContextFactory,
            ITextAnalyzer textAnalyzer,            
            ILogger<TextProcessingService> logger)
            : base(dataContextFactory, textAnalyzer, logger)
        { }

        protected override async Task<List<string>> AddUniqueWordsAsync(IWordsDataContext db, List<string> words)
        {
            var result = await db.WordsRepository.TryAddNewWordsAsync(words);
            var uniqueWords = result
                .Select(x => words[x.RowId - 1])
                .ToList();

            return uniqueWords;
        }
    }
}

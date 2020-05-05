namespace UniqueWords.Application.Words
{
    using TextAnalyzers;
    using Models;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using UniqueWords.Domain.Entities;

    public class TextProcessingService : ITextProcessingService
    {
        private readonly ITextProcessingDataContextFactory _dataContextFactory;
        private readonly ITextAnalyzer _textAnalyzer;
        private readonly ILogger<TextProcessingService> _logger;

        public TextProcessingService(
            ITextProcessingDataContextFactory dataContextFactory,
            ITextAnalyzer textAnalyzer,
            ILogger<TextProcessingService> logger)
        {
            _dataContextFactory = dataContextFactory;
            _textAnalyzer = textAnalyzer;
            _logger = logger;
        }

        public async Task<ProcessedTextResult> ProcessTextAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException(nameof(text));
            }

            try
            {
                return await AnalyzeTextTokensAsync(text);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while processing text.");
                throw;
            }
        }

        private async Task<ProcessedTextResult> AnalyzeTextTokensAsync(string text)
        {
            int uniqueWordsCount;
            List<string> watchWords;

            var distinctTokens = GetDistinctTokens(text);

            using (var db = _dataContextFactory.Create())
            {
                var uniqueWords = await db.WordsRepository.AddNewWordsAsync(distinctTokens);
                watchWords = await FindWatchWordMatchesAsync(db.WatchWordsRepository, distinctTokens); 

                uniqueWordsCount = uniqueWords.Count;
            }
           
            var result = new ProcessedTextResult
            {
                DistinctWords = distinctTokens.Count,
                DistinctUniqueWords = uniqueWordsCount,
                WatchlistWords = watchWords
            };

            return result;
        }


        private async Task<List<string>> FindWatchWordMatchesAsync(IWatchWordsRepository watchWordsRepository, List<string> words)
        {
            var matches = await watchWordsRepository.FindAsync(words);
            var wordMatches = matches
                .Select(ww => ww.Word)
                .ToList();

            return wordMatches;
        }

        private List<string> GetDistinctTokens(string text)
        {
            var tokens = _textAnalyzer.GetTokens(text);
            var distinctTokens = tokens
                .Distinct()
                .ToList();

            return distinctTokens;
        }
    }
}

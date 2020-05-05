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
        private readonly Lazy<Task<List<string>>> _watchWordsLazy;
        private readonly ILogger<TextProcessingService> _logger;

        public TextProcessingService(
            ITextProcessingDataContextFactory dataContextFactory,
            ITextAnalyzer textAnalyzer,
            ILogger<TextProcessingService> logger)
        {
            _dataContextFactory = dataContextFactory;
            _textAnalyzer = textAnalyzer;
            _watchWordsLazy = new Lazy<Task<List<string>>>(WatchWordsValueFactoryAsync);
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
                watchWords = await FindWatchWordMatchesAsync(distinctTokens);

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


        private async Task<List<string>> FindWatchWordMatchesAsync(List<string> words)
        {
            var watchWords = await _watchWordsLazy.Value;

            var matches = watchWords
                .Intersect(words)
                .ToList();

            return matches;
        }

        private async Task<List<string>> WatchWordsValueFactoryAsync()
        {
            List<WatchWordItem> watchWords;

            using (var db = _dataContextFactory.Create())
            {
                watchWords = await db.WatchWordsRepository.GetAllAsync();
            }

            var result = watchWords
                .Select(ww => ww.Word)
                .ToList();

            return result;
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

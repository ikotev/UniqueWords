using UniqueWords.Application.TextProcessing.TextAnalyzers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UniqueWords.Application.LazyInitialization;
using UniqueWords.Application.StartupConfigs;
using System.Threading;

namespace UniqueWords.Application.TextProcessing
{
    public class TextProcessingService : ITextProcessingService, IStartupTask
    {
        private static LazyValueFactory<List<string>> _watchWordsLazy = new LazyValueFactory<List<string>>();        

        private readonly ITextProcessingDataContextFactory _dataContextFactory;
        private readonly ITextAnalyzer _textAnalyzer;
        private readonly IUniqueWordsAddingStrategy _uniqueWorsAddingStrategy;
        private readonly ILogger<TextProcessingService> _logger;

        protected TextProcessingService(
            ITextProcessingDataContextFactory dataContextFactory,
            IUniqueWordsAddingStrategy uniqueWorsAddingStrategy,
            ITextAnalyzer textAnalyzer,
            ILogger<TextProcessingService> logger)
        {
            _dataContextFactory = dataContextFactory;
            _uniqueWorsAddingStrategy = uniqueWorsAddingStrategy;
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
                return await AnalyzeTextAsync(text);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while processing text.");
                throw;
            }
        }        

        private async Task<ProcessedTextResult> AnalyzeTextAsync(string text)
        {
            int uniqueWordsCount;
            List<string> watchWords;

            var distinctTokens = GetDistinctTokens(text);

            using (var db = _dataContextFactory.Create())
            {
                var uniqueWords = await _uniqueWorsAddingStrategy.AddUniqueWordsAsync(db.WordsRepository, distinctTokens);
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
            var watchWords = await GetWatchWordsAsync(watchWordsRepository);

            var matches = watchWords
                .Intersect(words)
                .ToList();

            return matches;
        }

        private List<string> GetDistinctTokens(string text)
        {
            var tokens = _textAnalyzer.GetTokens(text);
            var distinctTokens = tokens
                .Distinct()
                .ToList();

            return distinctTokens;
        }

        private async Task<List<string>> LoadWatchWordsAsync(IWatchWordsRepository watchWordsRepository)
        {
            var watchWords = await watchWordsRepository.GetAllAsync();

            var result = watchWords
                .Select(ww => ww.Word)
                .ToList();

            return result;
        }

        private async Task<List<string>> GetWatchWordsAsync(IWatchWordsRepository watchWordsRepository)
        {
            var watchWords = await _watchWordsLazy.GetValueAsync(() => LoadWatchWordsAsync(watchWordsRepository));
            return watchWords;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var db = _dataContextFactory.Create())
            {
                await GetWatchWordsAsync(db.WatchWordsRepository);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
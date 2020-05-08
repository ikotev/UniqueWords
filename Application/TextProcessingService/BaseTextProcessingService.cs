using UniqueWords.Application.TextProcessing.TextAnalyzers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UniqueWords.Domain.Entities;
using System.Threading;
using UniqueWords.Application.StartupConfigs;

namespace UniqueWords.Application.TextProcessing
{
    public abstract class BaseTextProcessingService<T> : ITextProcessingService, IStartupTask
        where T : BaseTextProcessingService<T>
    {
        private static volatile Task<List<string>> _watchWordsCache;
        private static object _watchWordsCacheLock = new object();

        private readonly ITextProcessingDataContextFactory _dataContextFactory;
        private readonly ITextAnalyzer _textAnalyzer;
        private readonly ILogger<T> _logger;

        protected BaseTextProcessingService(
            ITextProcessingDataContextFactory dataContextFactory,
            ITextAnalyzer textAnalyzer,
            ILogger<T> logger)
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
                var uniqueWords = await AddUniqueWordsAsync(db, distinctTokens);
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

        protected abstract Task<List<string>> AddUniqueWordsAsync(IWordsDataContext db, List<string> words);

        private async Task<List<string>> FindWatchWordMatchesAsync(List<string> words)
        {
            var watchWords = await GetWatchWordsCacheTask();

            var matches = watchWords
                .Intersect(words)
                .ToList();

            return matches;
        }

        private Task<List<string>> GetWatchWordsCacheTask(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_watchWordsCache == null)
            {
                lock (_watchWordsCacheLock)
                {
                    if (_watchWordsCache == null)
                    {
                        _watchWordsCache = LoadWatchWordsAsync(cancellationToken);
                    }
                }
            }

            return _watchWordsCache;
        }

        private async Task<List<string>> LoadWatchWordsAsync(CancellationToken cancellationToken)
        {
            List<WatchWordItem> watchWords;

            using (var db = _dataContextFactory.Create())
            {
                watchWords = await db.WatchWordsRepository.GetAllAsync(cancellationToken);
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

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await GetWatchWordsCacheTask(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
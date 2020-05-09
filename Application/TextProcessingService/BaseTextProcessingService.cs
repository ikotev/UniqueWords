using UniqueWords.Application.TextProcessing.TextAnalyzers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Threading;
using UniqueWords.Domain.Entities;

namespace UniqueWords.Application.TextProcessing
{
    public abstract class BaseTextProcessingService<T> : ITextProcessingService
        where T : BaseTextProcessingService<T>
    {        
        private static volatile List<string> _watchWords;
        private static SemaphoreSlim _signal = new SemaphoreSlim(1, 1);

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
            var watchWords = await GetWatchWordsAsync();

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

        private async Task<List<string>> GetWatchWordsAsync()
        {
            if (_watchWords == null)
            {
                await _signal.WaitAsync();

                try
                {
                    if (_watchWords == null)
                    {
                        _watchWords = await LoadWatchWordsAsync();
                    }
                }
                finally
                {
                    _signal.Release();
                }
            }

            return _watchWords;
        }   

        private async Task<List<string>> LoadWatchWordsAsync()
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
    }
}
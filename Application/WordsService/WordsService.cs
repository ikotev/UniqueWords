namespace UniqueWords.Application.Words
{
    using TextAnalyzers;
    using Models;

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using UniqueWords.Domain.Entities;

    public class WordsService : IWordsService
    {
        private readonly IWordsDataContextFactory _dataContextFactory;
        private readonly ITextAnalyzer _textAnalyzer;
        private readonly ILogger<WordsService> _logger;

        public WordsService(
            IWordsDataContextFactory dataContextFactory,
            ITextAnalyzer textAnalyzer,
            ILogger<WordsService> logger)
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

        public async Task<ProcessedTextResult> ProcessTextV2Async(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException(nameof(text));
            }

            try
            {
                return await AnalyzeTextTokensV2Async(text);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while processing text.");
                throw;
            }
        }

        private async Task<ProcessedTextResult> AnalyzeTextTokensV2Async(string text)
        {
            var tokens = _textAnalyzer.GetTokens(text);
            var distinctTokens = tokens
                .Distinct()
                .ToList();
            List<WatchWordItem> watchWords;

            using (var db = _dataContextFactory.Create())
            {
                await AddNewWordsV2Async(db.WordsRepository, distinctTokens);

                watchWords = await db.WatchListRepository.FindAsync(distinctTokens);
            }

            var watchList = watchWords
                .Select(ww => ww.Word)
                .ToList();
            var result = new ProcessedTextResult
            {
                DistinctUniqueWords = distinctTokens.Count,
                WatchlistWords = watchList
            };

            return result;
        }

        private async Task AddNewWordsV2Async(IWordsRepository wordsRepository, List<string> words)
        {
            var response = await wordsRepository.AddNewWordsV2Async(words);

            var n = response.Count;
        }

        private async Task<ProcessedTextResult> AnalyzeTextTokensAsync(string text)
        {
            var tokens = _textAnalyzer.GetTokens(text);
            var distinctTokens = tokens.Distinct().ToList();
            List<WatchWordItem> watchWords;

            using (var db = _dataContextFactory.Create())
            {
                await AddNewWordsAsync(db, distinctTokens);

                watchWords = await db.WatchListRepository.FindAsync(distinctTokens);
            }

            var watchList = watchWords
                .Select(ww => ww.Word)
                .ToList();
            var result = new ProcessedTextResult
            {
                DistinctUniqueWords = distinctTokens.Count,
                WatchlistWords = watchList
            };

            return result;
        }

        private async Task AddNewWordsAsync(IWordsDataContext db, List<string> words)
        {
            var skip = 0;
            var take = 100;
            var n = words.Count;

            while (skip < n)
            {
                var part = words
                .Skip(skip)
                .Take(take)
                .ToList();

                using (var transaction = await db.BeginTransactionAsync(IsolationLevel.Serializable))
                {
                    var newWords = await FindNewWordsAsync(db.WordsRepository, part);
                    var newWordItems = newWords
                        .Select(w => new WordItem { Word = w })
                        .ToList();
                    await db.WordsRepository.AddWordsAsync(newWordItems);
                    await db.SaveChangesAsync();

                    transaction.Commit();
                }
                skip += take;
            }
        }

        private static async Task<List<string>> FindNewWordsAsync(IWordsRepository wordsRepository, List<string> words)
        {
            var matchedWords = await wordsRepository.FindAsync(words);
            var newWords = words
                .Except(matchedWords.Select(wi => wi.Word))
                .ToList();

            return newWords;
        }
    }
}

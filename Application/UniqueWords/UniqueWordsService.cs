namespace UniqueWords.Application.UniqueWords
{
    using Domain.Entities;

    using Interfaces;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using Models;

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Data;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class UniqueWordsService : IUniqueWordsService
    {
        private readonly IUniqueWordsDbContextFactory _contextFactory;
        private readonly ITextAnalyzer _textAnalyzer;
        private readonly Lazy<ConcurrentBag<string>> _watchListLazy;
        private readonly ILogger<UniqueWordsService> _logger;

        private ConcurrentBag<string> _watchList => _watchListLazy.Value;

        public UniqueWordsService(
            IUniqueWordsDbContextFactory contextFactory,
            ITextAnalyzer textAnalyzer,
            ILogger<UniqueWordsService> logger)
        {
            _contextFactory = contextFactory;
            _textAnalyzer = textAnalyzer;
            _logger = logger;

            _watchListLazy = new Lazy<ConcurrentBag<string>>(WatchListFactory, LazyThreadSafetyMode.ExecutionAndPublication);
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
            var tokens = _textAnalyzer.GetTokens(text);
            var distinctTokens = tokens.Distinct().ToList();
            
            using (var db = _contextFactory.CreateDbContext())
            {
                await AddNewWordsAsync(db, distinctTokens);
            }

            var watchListMatches = FindWatchListMatches(distinctTokens);

            var result = new ProcessedTextResult
            {
                DistinctUniqueWords = distinctTokens.Count,
                WatchlistWords = watchListMatches
            };

            return result;
        }

        private async Task AddNewWordsAsync(IUniqueWordsDbContext db, List<string> words)
        {
            var skip = 0;
            var take = 100;
            var n = words.Count;

            while (skip < n)
            {
                var part = words.Skip(skip).Take(take);

                using (var transaction = await db.BeginTransactionAsync(IsolationLevel.Serializable))
                {
                    var newWords = await FindNewWordsAsync(db, part);

                    await db.Words.AddRangeAsync(newWords);
                    await db.SaveChangesAsync();

                    transaction.Commit();
                }
                skip += take;
            }
        }

        private static async Task<IEnumerable<WordItem>> FindNewWordsAsync(IUniqueWordsDbContext db, IEnumerable<string> words)
        {
            var matchedWordsQuery = from w in db.Words
                                    where words.Contains(w.Word)
                                    select w.Word;

            var matchedWords = await matchedWordsQuery
                .AsNoTracking()
                .ToListAsync();

            var newWords = words.Except(matchedWords)
                .Select(w => new WordItem { Word = w });

            return newWords;
        }

        private List<string> FindWatchListMatches(IEnumerable<string> tokens)
        {
            return _watchList.Intersect(tokens).ToList();
        }

        private ConcurrentBag<string> WatchListFactory()
        {
            using (var db = _contextFactory.CreateDbContext())
            {
                var watchListQuery = from wl in db.WatchList
                                     select wl.Word;

                var watchList = watchListQuery
                    .AsNoTracking();

                return new ConcurrentBag<string>(watchList);
            }
        }
    }
}

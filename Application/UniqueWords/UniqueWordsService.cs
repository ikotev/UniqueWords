namespace UniqueWords.Application.UniqueWords
{
    using Domain.Entities;

    using Interfaces;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using Models;

    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    public class UniqueWordsService : IUniqueWordsService
    {
        private readonly IUniqueWordsDbContextFactory _contextFactory;
        private readonly ITextAnalyzer _textAnalyzer;
        private readonly ILogger<UniqueWordsService> _logger;

        public UniqueWordsService(
            IUniqueWordsDbContextFactory contextFactory,
            ITextAnalyzer textAnalyzer,
            ILogger<UniqueWordsService> logger)
        {
            _contextFactory = contextFactory;
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
            var tokens = _textAnalyzer.GetTokens(text);
            var distinctTokens = tokens.Distinct().ToList();
            List<string> watchListMatches;

            using (var db = _contextFactory.CreateDbContext())
            {
                await AddNewWordsAsync(db, distinctTokens);

                watchListMatches = await FindWatchListMatchesAsync(db, distinctTokens);
            }

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

        private async Task<List<string>> FindWatchListMatchesAsync(IUniqueWordsDbContext db, IEnumerable<string> tokens)
        {
            var watchListQuery = from wl in db.WatchList
                                 where tokens.Contains(wl.Word)
                                 select wl.Word;

            var watchListMatches = await watchListQuery
                .AsNoTracking()
                .ToListAsync();

            return watchListMatches;
        }
    }
}

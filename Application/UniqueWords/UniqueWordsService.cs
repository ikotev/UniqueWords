namespace UniqueWords.Application.UniqueWords
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Models;

    public class UniqueWordsService : IUniqueWordsService
    {
        private readonly IUniqueWordsDbContext _context;
        private readonly ITextAnalyzer _textAnalyzer;
        private readonly ILogger<UniqueWordsService> _logger;

        public UniqueWordsService(
            IUniqueWordsDbContext context,
            ITextAnalyzer textAnalyzer,
            ILogger<UniqueWordsService> logger)
        {
            _context = context;
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

            await AddNewWordsAsync(distinctTokens);

            var watchListMatches = await FindWatchListMatchesAsync(distinctTokens);

            var result = new ProcessedTextResult
            {
                DistinctUniqueWords = distinctTokens.Count,
                WatchlistWords = watchListMatches
            };

            return result;
        }

        private async Task AddNewWordsAsync(List<string> words)
        {
            var matchedWordsQuery = from w in _context.Words
                                    where words.Contains(w.Word)
                                    select w.Word;

            var matchedWords = await matchedWordsQuery.AsNoTracking().ToListAsync();

            var newWords = words.Except(matchedWords)
                .Select(w => new WordItem { Word = w });

            await _context.Words.AddRangeAsync(newWords);
            await _context.SaveChangesAsync();
        }

        private async Task<List<string>> FindWatchListMatchesAsync(IEnumerable<string> tokens)
        {
            var watchListQuery = from wl in _context.WatchList
                                 where tokens.Contains(wl.Word)
                                 select wl.Word;

            var watchListMatches = await watchListQuery
                .AsNoTracking()
                .ToListAsync();

            return watchListMatches;
        }
    }
}

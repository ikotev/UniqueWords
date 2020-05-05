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
            int newWordsCount;
            List<WatchWordItem> watchWords;

            var tokens = _textAnalyzer.GetTokens(text);
            var distinctTokens = tokens
                .Distinct()
                .ToList();            

            using (var db = _dataContextFactory.Create())
            {
                newWordsCount = await AddNewWordsV2Async(db.WordsRepository, distinctTokens);                
                watchWords = await db.WatchListRepository.FindAsync(distinctTokens);                 
            }

            var watchList = watchWords
                .Select(ww => ww.Word)
                .ToList();

            var result = new ProcessedTextResult
            {
                DistinctWords = distinctTokens.Count,
                DistinctUniqueWords = newWordsCount,
                WatchlistWords = watchList
            };

            return result;
        }

        private async Task<int> AddNewWordsV2Async(IWordsRepository wordsRepository, List<string> words)
        {
            var response = await wordsRepository.AddNewWordsV2Async(words);

            return response.Count;
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

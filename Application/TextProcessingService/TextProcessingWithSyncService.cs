﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UniqueWords.Application.TextProcessing.TextAnalyzers;
using UniqueWords.Application.UniqueWordsWorkItem;
using UniqueWords.Application.WorkQueue;

namespace UniqueWords.Application.TextProcessing
{
    public class TextProcessingWithSyncService : BaseTextProcessingService<TextProcessingWithSyncService>
    {
        private readonly IBackgroundWorkQueuePublisher<UniqueWordsMessage> _workQueuePublisher;

        public TextProcessingWithSyncService(
            ITextProcessingDataContextFactory dataContextFactory,
            ITextAnalyzer textAnalyzer,
            IBackgroundWorkQueuePublisher<UniqueWordsMessage> workQueue,
            ILogger<TextProcessingWithSyncService> logger)
            : base(dataContextFactory, textAnalyzer, logger)
        {
            _workQueuePublisher = workQueue;
        }

        protected override async Task<List<string>> AddUniqueWordsAsync(IWordsDataContext db, List<string> words)
        {
            var uniqueWords = await FindUniqueWordsAsync(db.WordsRepository, words);

            if (uniqueWords.Any())
            {
                _workQueuePublisher.Publish(new UniqueWordsMessage(uniqueWords));
            }

            return uniqueWords;
        }

        private async Task<List<string>> FindUniqueWordsAsync(IWordsRepository wordsRepository, List<string> words)
        {
            var skip = 0;
            var take = 100;
            var n = words.Count;
            var uniqueWords = new List<string>();

            while (skip < n)
            {
                var wordsPart = words
                .Skip(skip)
                .Take(take)
                .ToList();

                var foundWords = await wordsRepository.FindAsync(wordsPart);
                var uniqueWordsPart = wordsPart
                .Except(foundWords.Select(wi => wi.Word));

                uniqueWords.AddRange(uniqueWordsPart);

                skip += take;
            }

            return uniqueWords;
        }
    }
}

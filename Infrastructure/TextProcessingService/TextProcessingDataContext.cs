using UniqueWords.Application.Words;
using UniqueWords.Infrastructure.Persistence;

namespace UniqueWords.Infrastructure.TextProcessing
{
    public class TextProcessingDataContext : BaseDataContext<UniqueWordsDbContext>, IWordsDataContext
    {
        public IWordsRepository WordsRepository { get; }

        public IWatchWordsRepository WatchListRepository { get; }

        public TextProcessingDataContext(IDbContextFactory<UniqueWordsDbContext> dbContextFactory)
        : base(dbContextFactory)
        {
            WordsRepository = new WordsRepository(DbContext);
            WatchListRepository = new WatchWordsRepository(DbContext);
        }
    }
}
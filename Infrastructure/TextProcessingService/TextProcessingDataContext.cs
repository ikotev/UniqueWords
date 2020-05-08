using UniqueWords.Application.TextProcessing;
using UniqueWords.Infrastructure.Persistence;

namespace UniqueWords.Infrastructure.TextProcessing
{
    public class TextProcessingDataContext : BaseDataContext<UniqueWordsDbContext>, IWordsDataContext
    {
        public IWordsRepository WordsRepository { get; }

        public IWatchWordsRepository WatchWordsRepository { get; }

        public TextProcessingDataContext(IDbContextFactory<UniqueWordsDbContext> dbContextFactory)
        : base(dbContextFactory)
        {
            WordsRepository = new WordsRepository(DbContext);
            WatchWordsRepository = new WatchWordsRepository(DbContext);
        }
    }
}
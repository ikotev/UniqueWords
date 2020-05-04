using UniqueWords.Application.Words;
using UniqueWords.Infrastructure.Persistence;

namespace UniqueWords.Infrastructure.Words
{
    public class WordsDataContext : BaseDataContext<UniqueWordsDbContext>, IWordsDataContext
    {
        public IWordsRepository WordsRepository { get; }

        public IWatchWordsRepository WatchListRepository { get; }

        public WordsDataContext(IDbContextFactory<UniqueWordsDbContext> dbContextFactory)
        : base(dbContextFactory)
        {
            WordsRepository = new WordsRepository(DbContext);
            WatchListRepository = new WatchWordsRepository(DbContext);
        }
    }
}
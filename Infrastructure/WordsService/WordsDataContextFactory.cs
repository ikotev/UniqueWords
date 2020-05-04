using UniqueWords.Application.Words;
using UniqueWords.Infrastructure.Persistence;

namespace UniqueWords.Infrastructure.Words
{
    public class WordsDataContextFactory : IWordsDataContextFactory
    {
        private readonly IDbContextFactory<UniqueWordsDbContext> _dbContextFactory;

        public WordsDataContextFactory(IDbContextFactory<UniqueWordsDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public IWordsDataContext Create()
        {
            return new WordsDataContext(_dbContextFactory);
        }
    }
}
using UniqueWords.Application.WordsAdding;
using UniqueWords.Infrastructure.Persistence;

namespace UniqueWords.Infrastructure.WordsAdding
{
    public class WordsAddingDataContextFactory : IWordsAddingDataContextFactory
    {
        private readonly IDbContextFactory<UniqueWordsDbContext> _dbContextFactory;

        public WordsAddingDataContextFactory(IDbContextFactory<UniqueWordsDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public IWordsAddingDataContext Create()
        {
            return new WordsAddingDataContext(_dbContextFactory);
        }
    }    
}
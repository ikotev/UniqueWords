using UniqueWords.Application.TextProcessing;
using UniqueWords.Infrastructure.Persistence;

namespace UniqueWords.Infrastructure.TextProcessing
{
    public class TextProcessingDataContextFactory : ITextProcessingDataContextFactory
    {
        private readonly IDbContextFactory<UniqueWordsDbContext> _dbContextFactory;

        public TextProcessingDataContextFactory(IDbContextFactory<UniqueWordsDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public IWordsDataContext Create()
        {
            return new TextProcessingDataContext(_dbContextFactory);
        }
    }
}
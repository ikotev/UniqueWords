using UniqueWords.Application.TextProcessing;
using UniqueWords.Application.WordsAdding;
using UniqueWords.Infrastructure.Persistence;
using UniqueWords.Infrastructure.Respositories;

namespace UniqueWords.Infrastructure.WordsAdding
{
    public class WordsAddingDataContext : BaseDataContext<UniqueWordsDbContext>, IWordsAddingDataContext
    {
        public IWordsRepository WordsRepository { get; }        

        public WordsAddingDataContext(IDbContextFactory<UniqueWordsDbContext> dbContextFactory)
        : base(dbContextFactory)
        {
            WordsRepository = new WordsRepository(DbContext);            
        }
    }
}
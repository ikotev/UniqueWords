using UniqueWords.Application.Persistence;
using UniqueWords.Application.TextProcessing;

namespace UniqueWords.Application.WordsAdding
{
    public interface IWordsAddingDataContext : IDataContext
    {
        IWordsRepository WordsRepository { get; }        
    }
}
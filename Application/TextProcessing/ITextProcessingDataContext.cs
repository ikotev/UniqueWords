using UniqueWords.Application.Persistence;

namespace UniqueWords.Application.TextProcessing
{
    public interface IWordsDataContext : IDataContext
    {
        IWordsRepository WordsRepository { get; }

        IWatchWordsRepository WatchWordsRepository { get; }
    }
}
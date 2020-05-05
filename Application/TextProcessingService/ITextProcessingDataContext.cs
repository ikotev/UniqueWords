using UniqueWords.Application.Persistence;

namespace UniqueWords.Application.Words
{
    public interface IWordsDataContext : IDataContext
    {
        IWordsRepository WordsRepository { get; }

        IWatchWordsRepository WatchWordsRepository { get; }
    }
}
namespace UniqueWords.Application.WordsAdding
{
    public interface IWordsAddingDataContextFactory
    {        
        IWordsAddingDataContext Create();
    }
}
namespace UniqueWords.Application.Words
{
    public interface IWordsDataContextFactory
    {        
        IWordsDataContext Create();
    }
}
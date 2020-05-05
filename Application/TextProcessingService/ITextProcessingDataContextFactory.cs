namespace UniqueWords.Application.Words
{
    public interface ITextProcessingDataContextFactory
    {        
        IWordsDataContext Create();
    }
}
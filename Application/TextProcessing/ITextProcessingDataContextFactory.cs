namespace UniqueWords.Application.TextProcessing
{
    public interface ITextProcessingDataContextFactory
    {        
        IWordsDataContext Create();
    }
}
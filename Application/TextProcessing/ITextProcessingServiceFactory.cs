namespace UniqueWords.Application.TextProcessing
{
    public interface ITextProcessingServiceFactory
    {
        ITextProcessingService Create<TAddingStrategy>() where TAddingStrategy : IUniqueWordsAddingStrategy;
    }
}
namespace UniqueWords.Application.TextProcessing
{
    using Models;

    using System.Threading.Tasks;

    public interface ITextProcessingService
    {        
        Task<ProcessedTextResult> ProcessTextAsync(string text);
    }
}

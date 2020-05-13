using System.Threading.Tasks;

namespace UniqueWords.Application.TextProcessing
{
    public interface ITextProcessingService
    {
        Task<ProcessedTextResult> ProcessTextAsync(string text);
    }
}

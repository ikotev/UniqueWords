namespace UniqueWords.Application.Words
{
    using Models;

    using System.Threading.Tasks;

    public interface IWordsService
    {
        Task<ProcessedTextResult> ProcessTextAsync(string text);

        Task<ProcessedTextResult> ProcessTextV2Async(string text);
    }
}

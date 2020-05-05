namespace UniqueWords.Application.Words
{
    using Models;

    using System.Threading.Tasks;

    public interface IWordsService
    {        
        Task<ProcessedTextResult> ProcessTextAsync(string text);
    }
}

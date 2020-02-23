namespace UniqueWords.Application.Interfaces
{
    using System.Threading.Tasks;
    using Models;

    public interface IUniqueWordsService
    {
        Task<ProcessedTextResult> ProcessTextAsync(string text);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniqueWords.Application.TextProcessing
{
    public interface IUniqueWordsAddingStrategy
    {
        Task<List<string>> AddUniqueWordsAsync(IWordsRepository wordsRepository, List<string> words);
    }
}

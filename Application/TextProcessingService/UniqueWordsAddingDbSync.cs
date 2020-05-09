using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniqueWords.Application.TextProcessing
{
    public class UniqueWordsAddingDbSync : IUniqueWordsAddingStrategy
    {        
        public async Task<List<string>> AddUniqueWordsAsync(IWordsRepository wordsRepository, List<string> words)
        {
            var result = await wordsRepository.TryAddNewWordsAsync(words);
            var uniqueWords = result
                .Select(x => words[x.RowId - 1])
                .ToList();

            return uniqueWords;
        }
    }
}

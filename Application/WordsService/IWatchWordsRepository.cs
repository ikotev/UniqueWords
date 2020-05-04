using System.Collections.Generic;
using System.Threading.Tasks;
using UniqueWords.Domain.Entities;

namespace UniqueWords.Application.Words
{
    public interface IWatchWordsRepository
    {                
        Task<List<WatchWordItem>> FindAsync(List<string> words);
    }
}
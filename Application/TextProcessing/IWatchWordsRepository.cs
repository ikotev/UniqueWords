using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UniqueWords.Domain.Entities;

namespace UniqueWords.Application.TextProcessing
{
    public interface IWatchWordsRepository
    {                
        Task<List<WatchWordItem>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));        
        Task<List<WatchWordItem>> FindAsync(List<string> words);
    }
}
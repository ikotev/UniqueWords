using System.Collections.Generic;
using UniqueWords.Application.TextProcessing;
using UniqueWords.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using UniqueWords.Domain.Entities;
using System.Threading;

namespace UniqueWords.Infrastructure.TextProcessing
{
    public class WatchWordsRepository : IWatchWordsRepository
    {
        private readonly UniqueWordsDbContext _dbContext;

        public WatchWordsRepository(UniqueWordsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<WatchWordItem>> GetAllAsync(CancellationToken cancellationToken)
        {
            var query = from wl in _dbContext.WatchList                        
                        select wl;

            var result = await query
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return result;
        }

        public async Task<List<WatchWordItem>> FindAsync(List<string> words)
        {
            var query = from wl in _dbContext.WatchList
                        where words.Contains(wl.Word)
                        select wl;

            var result = await query
                .AsNoTracking()
                .ToListAsync();

            return result;
        }
    }
}
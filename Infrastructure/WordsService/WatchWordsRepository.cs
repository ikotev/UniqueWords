using System.Collections.Generic;
using UniqueWords.Application.Words;
using UniqueWords.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using UniqueWords.Domain.Entities;

namespace UniqueWords.Infrastructure.Words
{
    public class WatchWordsRepository : IWatchWordsRepository
    {
        private readonly UniqueWordsDbContext _dbContext;

        public WatchWordsRepository(UniqueWordsDbContext dbContext)
        {
            _dbContext = dbContext;
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
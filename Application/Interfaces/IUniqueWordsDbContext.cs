namespace UniqueWords.Application.Interfaces
{
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public interface IUniqueWordsDbContext
    {
        DbSet<WordItem> Words { get; set; }

        DbSet<WatchWordItem> WatchList { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

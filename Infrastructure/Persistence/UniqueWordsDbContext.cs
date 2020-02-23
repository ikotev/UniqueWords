namespace UniqueWords.Infrastructure.Persistence
{
    using Application.Interfaces;

    using Domain.Entities;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;

    using System.Data;
    using System.Reflection;
    using System.Threading.Tasks;

    public class UniqueWordsDbContext : DbContext, IUniqueWordsDbContext
    {
        public UniqueWordsDbContext(DbContextOptions<UniqueWordsDbContext> options) : base(options)
        {
            
        }

        public DbSet<WordItem> Words { get; set; }

        public DbSet<WatchWordItem> WatchList { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        public Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel)
        {
            return base.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        }
    }
}

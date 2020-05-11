namespace UniqueWords.Infrastructure.Persistence
{    
    using Domain.Entities;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;

    using System.Data;
    using System.Reflection;
    using System.Threading.Tasks;
    using UniqueWords.Application.Models;

    public class UniqueWordsDbContext : DbContext
    {
        public UniqueWordsDbContext(DbContextOptions<UniqueWordsDbContext> options)
        : base(options)
        { }

        public DbSet<WordItem> Words { get; set; }

        public DbSet<WatchWordItem> WatchList { get; set; }

        public DbSet<AddNewWordsOutput> AddNewWords { get; set; }

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

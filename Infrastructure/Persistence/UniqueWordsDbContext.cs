namespace UniqueWords.Infrastructure.Persistence
{
    using System.Collections.Concurrent;
    using System.Reflection;
    using Application.Interfaces;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;

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
    }
}

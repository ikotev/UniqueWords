namespace UniqueWords.Infrastructure.Persistence
{
    using Application.Interfaces;

    using Microsoft.EntityFrameworkCore;

    public class UniqueWordsDbContextFactory : IUniqueWordsDbContextFactory
    {
        private readonly string _connectionString;

        public UniqueWordsDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IUniqueWordsDbContext CreateDbContext()
        {
            return new UniqueWordsDbContext(GetDbContextOptions(_connectionString));
        }

        private static DbContextOptions<UniqueWordsDbContext> GetDbContextOptions(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UniqueWordsDbContext>();

            optionsBuilder.UseSqlServer(connectionString);

            return optionsBuilder.Options;
        }
    }
}
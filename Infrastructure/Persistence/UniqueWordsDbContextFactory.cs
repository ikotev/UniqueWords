using Microsoft.EntityFrameworkCore;

namespace UniqueWords.Infrastructure.Persistence
{
    public class UniqueWordsDbContextFactory: IDbContextFactory<UniqueWordsDbContext>
    {
        private readonly DbContextOptions<UniqueWordsDbContext> _options;

        public UniqueWordsDbContextFactory(DbContextOptions<UniqueWordsDbContext> options)
        {
            _options = options;
        }

        public UniqueWordsDbContext Create()
        {
            return new UniqueWordsDbContext(_options);
        }        

        // private static DbContextOptions<UniqueWordsDbContext> GetDbContextOptions(string connectionString)
        // {
        //     var optionsBuilder = new DbContextOptionsBuilder<UniqueWordsDbContext>();

        //     optionsBuilder.UseSqlServer(connectionString);

        //     return optionsBuilder.Options;
        // }
    }    
}
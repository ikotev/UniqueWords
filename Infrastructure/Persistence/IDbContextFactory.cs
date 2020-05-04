using Microsoft.EntityFrameworkCore;

namespace UniqueWords.Infrastructure.Persistence
{
    public interface IDbContextFactory<TContext> where TContext : DbContext
    {        
        TContext Create();
    }
}
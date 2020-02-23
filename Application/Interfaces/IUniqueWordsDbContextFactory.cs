namespace UniqueWords.Application.Interfaces
{
    public interface IUniqueWordsDbContextFactory
    {
        IUniqueWordsDbContext CreateDbContext();
    }
}
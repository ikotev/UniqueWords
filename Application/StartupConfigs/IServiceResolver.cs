namespace UniqueWords.Application.StartupConfigs
{
    public interface IServiceResolver
    {
        TService GetService<TService, TImplementation>();
    }
}
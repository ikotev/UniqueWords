namespace UniqueWords.Application.StartupConfigs
{
    public interface IServiceResolver
    {
        TService GetService<TService>();
        
        TService GetService<TService, TImplementation>() where TImplementation : TService;
    }
}
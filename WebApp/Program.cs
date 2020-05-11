using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using UniqueWords.WebApp.StartupConfigs;

namespace UniqueWords.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .AddAzureKeyVaultProvider()                
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });        
    }
}

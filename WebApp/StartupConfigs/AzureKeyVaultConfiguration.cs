using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;

namespace UniqueWords.WebApp.StartupConfigs
{
    public static class AzureKeyVaultConfiguration
    {
        public static IHostBuilder AddAzureKeyVaultProvider(this IHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var builtConfig = config.Build();
                var section = builtConfig.GetSection("AzureKeyVault");

                if (section.Exists())
                {
                    var options = new AzureKeyVaultConfigurationOptions(
                                        section["vault"],
                                        section["clientId"],
                                        section["clientSecret"]);

                    config.AddAzureKeyVault(options);
                }
            });

            return builder;
        }
    }
}
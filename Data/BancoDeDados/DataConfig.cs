using Microsoft.Extensions.Configuration;

namespace Data.Config
{
    public static class ConfigHelper
    {
        public static IConfigurationRoot LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) 
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
 
            return builder.Build();
        }
    }
}
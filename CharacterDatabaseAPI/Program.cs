using CharacterDatabaseAPI.Services;
using Amazon.Extensions.NETCore.Setup;

namespace CharacterDatabaseAPI;
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(configurationBuilder =>
            {
                configurationBuilder.AddEnvironmentVariables();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

}

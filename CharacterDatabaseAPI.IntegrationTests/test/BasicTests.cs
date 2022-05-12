using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using CharacterDatabaseAPI.Services;
using Microsoft.Extensions.DependencyInjection;
namespace CharacterDatabaseAPI.IntegrationTests;

public class BasicTests
    : IClassFixture<WebApplicationFactory<Startup>>
{
    private const string TestUniverse = "StarWars";
    private const string TestName = "TestName";

    private readonly WebApplicationFactory<Startup> _factory;

    public BasicTests(WebApplicationFactory<Startup> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((context, config) =>
                    {
                        config.Add();
                    });
                });

    }

    [Theory]
    [InlineData("api/")]
    [InlineData($"api/characters/{TestUniverse}")]
    [InlineData($"api/characters/{TestUniverse}/get/{TestName}")]
    [InlineData($"api/characters/{TestUniverse}/search")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode(); // Status Code 200-299
        System.Console.WriteLine(await response.Content.ReadAsStringAsync());
    }
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CharacterDatabaseAPI.IntegrationTests;

public class BasicTests
    : IClassFixture<WebApplicationFactory<CharacterDatabaseAPIProgram>>
{
    private const string TestUniverse = "Test Universe";
    private const string TestName = "Test Name";

    private readonly WebApplicationFactory<CharacterDatabaseAPIProgram> _factory;

    public BasicTests(WebApplicationFactory<CharacterDatabaseAPIProgram> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData($"get/{TestUniverse}")]
    [InlineData($"get/{TestUniverse}/{TestName}")]
    [InlineData($"search/{TestUniverse}")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("text/html; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
    }
}

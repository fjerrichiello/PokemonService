using System.Net;
using FluentAssertions;

namespace Nvisia.PokemonReview.IntegrationTests.Api.Endpoints;

public class PokemonEndpointsTest : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;

    public PokemonEndpointsTest(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllPokemonWithValidParameters()
    {
        var response = await _httpClient.GetAsync("api/pokemon/1");

        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
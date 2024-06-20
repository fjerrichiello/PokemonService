using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Nvisia.PokemonReview.Api.Endpoints.Models;

namespace Nvisia.PokemonReview.IntegrationTests.Api.Endpoints;

public class UserEndpointsTest : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;

    public UserEndpointsTest(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task PostLoginWithValidParameters()
    {
        var response = await _httpClient.PostAsJsonAsync("api/user/login", new LoginRequest
        {
            UserName = "test",
            Password = "test"
        });

        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}
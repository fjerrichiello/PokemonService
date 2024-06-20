using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Moq;
using Moq.Contrib.HttpClient;
using Nvisia.PokemonReview.Api.Clients;
using Nvisia.PokemonReview.Api.Clients.Models;
using Nvisia.PokemonReview.Api.Options;

namespace Nvisia.PokemonReview.UnitTests.Clients;

public class PokeApiClientTestMoq
{
    private readonly IPokeApiClient _pokeApiClient;
    private readonly HttpMessageHandler _httpMessageHandler;
    private static readonly Fixture Fixture = new();

    private const string PokeApiBaseUrl = "http://localhost/api/v2/";
    private const string PokemonId = "1";
    private const string PokemonName = "bulbasaur";

    private const string GetPokemonByIdPath = $"pokemon/{PokemonId}";
    private const string GetPokemonByNamePath = $"pokemon/{PokemonName}";

    private const string GetPokemonByIdFullPath = $"{PokeApiBaseUrl}{GetPokemonByIdPath}";
    private const string GetPokemonByNameFullPath = $"{PokeApiBaseUrl}{GetPokemonByNamePath}";

    public PokeApiClientTestMoq()
    {
        _httpMessageHandler = Mock.Of<HttpMessageHandler>();
        var httpClientFactory = Mock.Get(_httpMessageHandler).CreateClientFactory();
        Mock.Get(httpClientFactory).Setup(x => x.CreateClient(new PokeApiOptions().Key)).Returns(
            new HttpClient(_httpMessageHandler)
            {
                BaseAddress = new Uri(PokeApiBaseUrl)
            });

        _pokeApiClient = new PokeApiClient(httpClientFactory);
    }

    [Fact]
    public async Task ShouldGetPokemonById()
    {
        var pokemonId = 1;
        var expectedPokemon = Fixture.Create<PokeApiResponse>();
        var expectedResponse = new HttpResponseMessage
            { StatusCode = HttpStatusCode.OK, Content = JsonContent.Create(expectedPokemon) };


        Mock.Get(_httpMessageHandler)
            .SetupRequest(HttpMethod.Get, GetPokemonByIdFullPath)
            .ReturnsAsync(expectedResponse);

        var actualPokemon = await _pokeApiClient.GetPokemonById(pokemonId);

        Mock.Get(_httpMessageHandler)
            .VerifyRequest(HttpMethod.Get, GetPokemonByIdFullPath, times: Times.Once());

        actualPokemon.Should().BeEquivalentTo(expectedPokemon);
    }

    [Fact]
    public async Task ShouldGetPokemonByName()
    {
        var pokemonName = "bulbasaur";
        var expectedPokemon = Fixture.Create<PokeApiResponse>();
        var expectedResponse = new HttpResponseMessage
            { StatusCode = HttpStatusCode.OK, Content = JsonContent.Create(expectedPokemon) };

        Mock.Get(_httpMessageHandler)
            .SetupRequest(HttpMethod.Get, GetPokemonByNameFullPath)
            .ReturnsAsync(expectedResponse);

        var actualPokemon = await _pokeApiClient.GetPokemonByName(pokemonName);

        Mock.Get(_httpMessageHandler)
            .VerifyRequest(HttpMethod.Get, GetPokemonByNameFullPath, times: Times.Once());

        Assert.Equivalent(expectedPokemon, actualPokemon);
    }
}
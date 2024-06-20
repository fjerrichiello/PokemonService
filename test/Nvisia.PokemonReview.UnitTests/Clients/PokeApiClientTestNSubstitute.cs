using System.Net;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Nvisia.PokemonReview.Api.Clients;
using Nvisia.PokemonReview.Api.Clients.Models;
using Nvisia.PokemonReview.Api.Options;
using Nvisia.PokemonReview.UnitTests.Utils;

namespace Nvisia.PokemonReview.UnitTests.Clients;

public class PokeApiClientTestNSubstitute
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

    public PokeApiClientTestNSubstitute()
    {
        _httpMessageHandler = Substitute.For<HttpMessageHandler>();
        var httpClientFactory = Substitute.For<IHttpClientFactory>();
        httpClientFactory.CreateClient(new PokeApiOptions().Key).Returns(new HttpClient(_httpMessageHandler)
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

        _httpMessageHandler.SetupRequest(HttpMethod.Get, GetPokemonByIdFullPath)
            .ReturnsResponse(HttpStatusCode.OK, expectedPokemon);

        var actualPokemon = await _pokeApiClient.GetPokemonById(pokemonId);

        _httpMessageHandler.ShouldHaveReceived(HttpMethod.Get, GetPokemonByIdFullPath);
        actualPokemon.Should().BeEquivalentTo(expectedPokemon);
    }

    [Fact]
    public async Task ShouldGetPokemonByName()
    {
        var pokemonName = "bulbasaur";
        var expectedPokemon = Fixture.Create<PokeApiResponse>();

        _httpMessageHandler.SetupRequest(HttpMethod.Get, GetPokemonByNameFullPath)
            .ReturnsResponse(HttpStatusCode.OK, expectedPokemon);

        var actualPokemon = await _pokeApiClient.GetPokemonByName(pokemonName);

        _httpMessageHandler.ShouldHaveReceived(HttpMethod.Get, GetPokemonByNameFullPath);
        Assert.Equivalent(expectedPokemon, actualPokemon);
    }
}
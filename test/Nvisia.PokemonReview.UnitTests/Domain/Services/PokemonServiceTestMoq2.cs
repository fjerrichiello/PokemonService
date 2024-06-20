using AutoBogus;
using AutoFixture;
using Dumpify;
using Moq;
using FluentAssertions;
using Nvisia.PokemonReview.Api.Domain.Services;
using Nvisia.PokemonReview.Api.Persistence.Repositories;
using Nvisia.PokemonReview.Api.Mappers;
using Nvisia.PokemonReview.Api.Clients;
using Nvisia.PokemonReview.Api.Domain.Models;
using Nvisia.PokemonReview.Api.Persistence.Models;
using Xunit.Abstractions;

namespace Nvisia.PokemonReview.UnitTests.Domain.Services;

public class PokemonServiceTestMoq2
{
    private readonly PokemonService _pokemonService;

    private readonly ITestOutputHelper _output;
    private readonly Mock<IPokemonRepository> _pokemonRepository;
    private readonly Mock<IPokemonMapper> _pokemonMapper;
    private readonly Mock<IPokeApiClient> _pokeApiClient;

    private static readonly Fixture Fixture = new();
    private static readonly IAutoFaker Faker = AutoFaker.Create();

    public PokemonServiceTestMoq2(ITestOutputHelper output)
    {
        _output = output;
        _pokemonRepository = new();
        _pokemonMapper = new();
        _pokeApiClient = new();
        _pokemonService = new PokemonService(_pokeApiClient.Object, _pokemonRepository.Object, _pokemonMapper.Object);
    }

    [Fact]
    public async Task CreatePokemon_PokemonExists()
    {
        // Arrange
        var pokemon = Fixture.Create<Pokemon>();
        _output.WriteLine(pokemon.DumpText());
        _pokemonRepository
            .Setup(x => x.PokemonExists(pokemon.Id, pokemon.Name))
            .ReturnsAsync(true);

        // act
        var result = await _pokemonService.CreatePokemon(pokemon);

        // assert
        Assert.True(result.IsError);
        Assert.Equal("The pokemon already exists in our system", result.FirstError.Description);

        _pokemonRepository
            .Verify(x => x.PokemonExists(pokemon.Id, pokemon.Name), Times.Once);

        _pokeApiClient
            .Verify(x => x.GetPokemonById(It.IsAny<int>()), Times.Never);

        _pokeApiClient
            .Verify(x => x.GetPokemonByName(It.IsAny<string>()), Times.Never);

        _pokemonMapper
            .Verify(x => x.PokemonToPokemonEntity(It.IsAny<Pokemon>()), Times.Never);

        _pokemonRepository
            .Verify(x => x.AddPokemon(It.IsAny<PokemonEntity>()), Times.Never);

        _pokemonMapper
            .Verify(x => x.PokemonEntityToPokemon(It.IsAny<PokemonEntity>()), Times.Never);
    }

    [Fact]
    public async Task CreatePokemon_PokemonExists_SecondExample()
    {
        // Arrange
        var pokemon = Faker.Generate<Pokemon>();
        _output.WriteLine(pokemon.DumpText());
        _pokemonRepository
            .Setup(x => x.PokemonExists(pokemon.Id, pokemon.Name))
            .ReturnsAsync(true);

        // act
        var result = await _pokemonService.CreatePokemon(pokemon);

        // assert
        result.IsError.Should().BeTrue();
        result.FirstError.Description.Should().Be("The pokemon already exists in our system");

        _pokemonRepository
            .Verify(x => x.PokemonExists(pokemon.Id, pokemon.Name), Times.Once);

        _pokeApiClient
            .Verify(x => x.GetPokemonById(It.IsAny<int>()), Times.Never);

        _pokeApiClient
            .Verify(x => x.GetPokemonByName(It.IsAny<string>()), Times.Never);

        _pokemonMapper
            .Verify(x => x.PokemonToPokemonEntity(It.IsAny<Pokemon>()), Times.Never);

        _pokemonRepository
            .Verify(x => x.AddPokemon(It.IsAny<PokemonEntity>()), Times.Never);

        _pokemonMapper
            .Verify(x => x.PokemonEntityToPokemon(It.IsAny<PokemonEntity>()), Times.Never);
    }
}
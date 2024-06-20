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

public class PokemonServiceTestMoq1
{
    private readonly PokemonService _pokemonService;
    
    private readonly ITestOutputHelper _output;
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IPokemonMapper _pokemonMapper;
    private readonly IPokeApiClient _pokeApiClient;
    
    private static readonly Fixture Fixture = new();
    private static readonly IAutoFaker Faker = AutoFaker.Create();

    public PokemonServiceTestMoq1(ITestOutputHelper output)
    {
        _output = output;
        _pokemonRepository = Mock.Of<IPokemonRepository>();
        _pokemonMapper = Mock.Of<IPokemonMapper>();
        _pokeApiClient = Mock.Of<IPokeApiClient>();
        _pokemonService = new PokemonService(_pokeApiClient, _pokemonRepository, _pokemonMapper);
    }

    [Fact]
    public async Task CreatePokemon_PokemonExists()
    {
        // Arrange
        var pokemon = Fixture.Create<Pokemon>();
        _output.WriteLine(pokemon.DumpText());
        Mock.Get(_pokemonRepository)
            .Setup(x => x.PokemonExists(pokemon.Id, pokemon.Name))
            .ReturnsAsync(true);

        // act
        var result = await _pokemonService.CreatePokemon(pokemon);

        // assert
        Assert.True(result.IsError);
        Assert.Equal("The pokemon already exists in our system", result.FirstError.Description);

        Mock.Get(_pokemonRepository)
            .Verify(x => x.PokemonExists(pokemon.Id, pokemon.Name), Times.Once);

        Mock.Get(_pokeApiClient)
            .Verify(x => x.GetPokemonById(It.IsAny<int>()), Times.Never);

        Mock.Get(_pokeApiClient)
            .Verify(x => x.GetPokemonByName(It.IsAny<string>()), Times.Never);

        Mock.Get(_pokemonMapper)
            .Verify(x => x.PokemonToPokemonEntity(It.IsAny<Pokemon>()), Times.Never);

        Mock.Get(_pokemonRepository)
            .Verify(x => x.AddPokemon(It.IsAny<PokemonEntity>()), Times.Never);

        Mock.Get(_pokemonMapper)
            .Verify(x => x.PokemonEntityToPokemon(It.IsAny<PokemonEntity>()), Times.Never);
    }

    [Fact]
    public async Task CreatePokemon_PokemonExists_SecondExample()
    {
        // Arrange
        var pokemon = Faker.Generate<Pokemon>();
        _output.WriteLine(pokemon.DumpText());
        Mock.Get(_pokemonRepository)
            .Setup(x => x.PokemonExists(pokemon.Id, pokemon.Name))
            .ReturnsAsync(true);

        // act
        var result = await _pokemonService.CreatePokemon(pokemon);

        // assert
        result.IsError.Should().BeTrue();
        result.FirstError.Description.Should().Be("The pokemon already exists in our system");

        Mock.Get(_pokemonRepository)
            .Verify(x => x.PokemonExists(pokemon.Id, pokemon.Name), Times.Once);

        Mock.Get(_pokeApiClient)
            .Verify(x => x.GetPokemonById(It.IsAny<int>()), Times.Never);

        Mock.Get(_pokeApiClient)
            .Verify(x => x.GetPokemonByName(It.IsAny<string>()), Times.Never);

        Mock.Get(_pokemonMapper)
            .Verify(x => x.PokemonToPokemonEntity(It.IsAny<Pokemon>()), Times.Never);

        Mock.Get(_pokemonRepository)
            .Verify(x => x.AddPokemon(It.IsAny<PokemonEntity>()), Times.Never);

        Mock.Get(_pokemonMapper)
            .Verify(x => x.PokemonEntityToPokemon(It.IsAny<PokemonEntity>()), Times.Never);
    }
}
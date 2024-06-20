using System.Text.Json;
using AutoBogus;
using AutoFixture;
using Dumpify;
using NSubstitute;
using FluentAssertions;
using Nvisia.PokemonReview.Api.Domain.Services;
using Nvisia.PokemonReview.Api.Persistence.Repositories;
using Nvisia.PokemonReview.Api.Mappers;
using Nvisia.PokemonReview.Api.Clients;
using Nvisia.PokemonReview.Api.Domain.Models;
using Nvisia.PokemonReview.Api.Persistence.Models;
using Xunit.Abstractions;

namespace Nvisia.PokemonReview.UnitTests.Domain.Services;

public class PokemonServiceTestNSubstitute
{
    private readonly PokemonService _pokemonService;

    private readonly ITestOutputHelper _output;
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IPokemonMapper _pokemonMapper;
    private readonly IPokeApiClient _pokeApiClient;

    private static readonly Fixture Fixture = new();
    private static readonly IAutoFaker Faker = AutoFaker.Create();

    public PokemonServiceTestNSubstitute(ITestOutputHelper output)
    {
        _output = output;
        _pokemonRepository = Substitute.For<IPokemonRepository>();
        _pokemonMapper = Substitute.For<IPokemonMapper>();
        _pokeApiClient = Substitute.For<IPokeApiClient>();
        _pokemonService = new PokemonService(_pokeApiClient, _pokemonRepository, _pokemonMapper);
    }

    [Fact]
    public async Task CreatePokemon_PokemonExists()
    {
        // Arrange
        var pokemon = Fixture.Create<Pokemon>();
        _output.WriteLine(pokemon.DumpText());

        _pokemonRepository
            .PokemonExists(pokemon.Id, pokemon.Name)
            .Returns(true);

        // act
        var result = await _pokemonService.CreatePokemon(pokemon);

        // assert
        result.IsError.Should().BeTrue();
        result.FirstError.Description.Should().Be("The pokemon already exists in our system");

        await _pokemonRepository
            .Received(1)
            .PokemonExists(pokemon.Id, pokemon.Name);

        await _pokeApiClient
            .DidNotReceive()
            .GetPokemonById(Arg.Any<int>());

        await _pokeApiClient
            .DidNotReceive()
            .GetPokemonByName(Arg.Any<string>());

        _pokemonMapper
            .DidNotReceive()
            .PokemonToPokemonEntity(Arg.Any<Pokemon>());

        await _pokemonRepository
            .DidNotReceive()
            .AddPokemon(Arg.Any<PokemonEntity>());

        _pokemonMapper
            .DidNotReceive()
            .PokemonEntityToPokemon(Arg.Any<PokemonEntity>());
    }

    [Fact]
    public async Task CreatePokemon_PokemonExists_SecondExample()
    {
        // Arrange
        var pokemon = Faker.Generate<Pokemon>();
        _output.WriteLine(pokemon.DumpText());
        _pokemonRepository.PokemonExists(pokemon.Id, pokemon.Name).Returns(true);

        // act
        var result = await _pokemonService.CreatePokemon(pokemon);

        // assert
        Assert.True(result.IsError);
        Assert.Equal("The pokemon already exists in our system", result.FirstError.Description);

        await _pokemonRepository
            .Received(1)
            .PokemonExists(pokemon.Id, pokemon.Name);

        await _pokeApiClient
            .DidNotReceive()
            .GetPokemonById(Arg.Any<int>());

        await _pokeApiClient
            .DidNotReceive()
            .GetPokemonByName(Arg.Any<string>());

        _pokemonMapper
            .DidNotReceive()
            .PokemonToPokemonEntity(Arg.Any<Pokemon>());

        await _pokemonRepository
            .DidNotReceive()
            .AddPokemon(Arg.Any<PokemonEntity>());

        _pokemonMapper
            .DidNotReceive()
            .PokemonEntityToPokemon(Arg.Any<PokemonEntity>());
    }
}
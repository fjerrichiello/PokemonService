using ErrorOr;
using Nvisia.PokemonReview.Api.Clients;
using Nvisia.PokemonReview.Api.Clients.Models;
using Nvisia.PokemonReview.Api.Domain.Models;
using Nvisia.PokemonReview.Api.Mappers;
using Nvisia.PokemonReview.Api.Persistence.Repositories;

namespace Nvisia.PokemonReview.Api.Domain.Services;

public class PokemonService : IPokemonService
{
    private readonly IPokeApiClient _pokeApiClient;
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IPokemonMapper _pokemonMapper;

    public PokemonService(IPokeApiClient pokeApiClient, IPokemonRepository pokemonRepository,
        IPokemonMapper pokemonMapper)
    {
        _pokeApiClient = pokeApiClient ?? throw new ArgumentNullException(nameof(pokeApiClient));
        _pokemonRepository = pokemonRepository ?? throw new ArgumentNullException(nameof(pokemonRepository));
        _pokemonMapper = pokemonMapper ?? throw new ArgumentNullException(nameof(pokemonMapper));
    }

    public async Task<ErrorOr<AllPokemonInfo>> GetAllPokemon(int? page, int? pageSize)
    {
        var pokemon = await _pokemonRepository.GetAllPokemon(page, pageSize);
        var count = await _pokemonRepository.Count();
        var pages = (int)Math.Ceiling((double)count / (pageSize ?? count));
        var last = (page ?? 1) == pages;

        return new AllPokemonInfo
        {
            Content = pokemon
                .Select(x => _pokemonMapper.PokemonEntityToPokemon(x))
                .ToList(),
            Last = last,
            Page = page ?? 1,
            PageSize = pageSize ?? count,
            TotalElements = count,
            TotalPages = pages
        };
    }

    public async Task<ErrorOr<Pokemon>> CreatePokemon(Pokemon pokemon)
    {
        var existingEntity = await _pokemonRepository.PokemonExists(pokemon.Id, pokemon.Name);
        if (existingEntity)
        {
            return Error.Failure(description: "The pokemon already exists in our system");
        }

        var pokemonFromApi = await GetPokemonFromPokeApi(pokemon.Id) ??
                             await GetPokemonFromPokeApi(name: pokemon.Name);

        if (pokemonFromApi is null)
        {
            return Error.NotFound(description: "No Pokemon with that name of id exists in the Pokemon world");
        }

        var entity = _pokemonMapper.PokemonToPokemonEntity(pokemonFromApi);
        var result = await _pokemonRepository.AddPokemon(entity);

        if (!pokemon.Equals(pokemonFromApi))
        {
            return Error.Conflict(description: "There is an issue with the details you sent us.");
        }

        return _pokemonMapper.PokemonEntityToPokemon(result);
    }

    public async Task<ErrorOr<Pokemon>> GetPokemonById(int id)
    {
        var existingEntity = await _pokemonRepository.GetPokemonById(id);
        if (existingEntity is not null)
        {
            return _pokemonMapper.PokemonEntityToPokemon(existingEntity);
        }

        var pokemonFromApi = await GetPokemonFromPokeApi(id);
        if (pokemonFromApi is null)
        {
            return Error.NotFound(description: "The pokemon was not found");
        }

        var entity = _pokemonMapper.PokemonToPokemonEntity(pokemonFromApi);
        var result = await _pokemonRepository.AddPokemon(entity);
        return _pokemonMapper.PokemonEntityToPokemon(result);
    }


    public async Task<ErrorOr<Pokemon>> GetPokemonByName(string name)
    {
        var existingEntity = await _pokemonRepository.GetPokemonByName(name);
        if (existingEntity is not null)
        {
            return _pokemonMapper.PokemonEntityToPokemon(existingEntity);
        }

        var pokemonFromApi = await GetPokemonFromPokeApi(name: name);
        if (pokemonFromApi is null)
        {
            return Error.NotFound(description: "The pokemon was not found");
        }

        var entity = _pokemonMapper.PokemonToPokemonEntity(pokemonFromApi);
        var result = await _pokemonRepository.AddPokemon(entity);
        return _pokemonMapper.PokemonEntityToPokemon(result);
    }

    private async Task<Pokemon?> GetPokemonFromPokeApi(int id = default, string? name = null)
    {
        PokeApiResponse? pokemonFromApi;

        if (id is not 0)
        {
            pokemonFromApi = await _pokeApiClient.GetPokemonById(id);
            return pokemonFromApi is null ? null : _pokemonMapper.PokeApiResponseToPokemon(pokemonFromApi);
        }

        if (name is null) return default;
        pokemonFromApi = await _pokeApiClient.GetPokemonByName(name);

        return pokemonFromApi is null ? null : _pokemonMapper.PokeApiResponseToPokemon(pokemonFromApi);
    }
}
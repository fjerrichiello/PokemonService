using Nvisia.PokemonReview.Api.Clients.Models;
using Nvisia.PokemonReview.Api.Domain.Models;
using Nvisia.PokemonReview.Api.Endpoints.Models;
using Nvisia.PokemonReview.Api.Persistence.Models;

namespace Nvisia.PokemonReview.Api.Mappers;

public interface IPokemonMapper
{
    Pokemon PokeApiResponseToPokemon(PokeApiResponse pokeApiResponse);

    Pokemon CreatePokemonRequestToPokemon(CreatePokemonRequest createPokemonRequest);

    Pokemon PokemonEntityToPokemon(PokemonEntity pokemonEntity);

    PokemonEntity PokemonToPokemonEntity(Pokemon pokemon);

    PokemonResponse PokemonToPokemonResponse(Pokemon pokemon);

    GetPokemonResponse PokemonToGetPokemonResponse(Pokemon pokemon);

    CreatePokemonResponse PokemonToCreatePokemonResponse(Pokemon pokemon);
    
    GetAllPokemonResponse AllPokemonInfoToGetAllPokemonResponse(AllPokemonInfo allPokemonInfo);
}
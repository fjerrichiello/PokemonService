using Nvisia.PokemonReview.Api.Clients.Models;
using Nvisia.PokemonReview.Api.Domain.Models;
using Nvisia.PokemonReview.Api.Endpoints.Models;
using Nvisia.PokemonReview.Api.Persistence.Models;
using Riok.Mapperly.Abstractions;

namespace Nvisia.PokemonReview.Api.Mappers;

[Mapper]
public partial class PokemonMapper : IPokemonMapper
{
    public partial Pokemon PokeApiResponseToPokemon(PokeApiResponse pokeApiResponse);

    public partial Pokemon CreatePokemonRequestToPokemon(CreatePokemonRequest createPokemonRequest);

    public partial Pokemon PokemonEntityToPokemon(PokemonEntity pokemonEntity);

    public partial PokemonEntity PokemonToPokemonEntity(Pokemon pokemon);

    public partial PokemonResponse PokemonToPokemonResponse(Pokemon pokemon);

    public partial GetPokemonResponse PokemonToGetPokemonResponse(Pokemon pokemon);

    public partial CreatePokemonResponse PokemonToCreatePokemonResponse(Pokemon pokemon);

    public partial GetAllPokemonResponse AllPokemonInfoToGetAllPokemonResponse(AllPokemonInfo allPokemonInfo);

    // ReSharper disable once ConvertClosureToMethodGroup
    private List<PokemonResponse> MapPokemonList(List<Pokemon> pokemon) =>
        pokemon.Select(x => PokemonToPokemonResponse(x)).ToList();
}
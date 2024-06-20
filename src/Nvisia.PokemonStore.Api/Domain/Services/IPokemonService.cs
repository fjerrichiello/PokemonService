using ErrorOr;
using Nvisia.PokemonReview.Api.Domain.Models;

namespace Nvisia.PokemonReview.Api.Domain.Services;

public interface IPokemonService
{
    Task<ErrorOr<AllPokemonInfo>> GetAllPokemon(int? page, int? pageSize);

    Task<ErrorOr<Pokemon>> CreatePokemon(Pokemon pokemon);

    Task<ErrorOr<Pokemon>> GetPokemonById(int id);

    Task<ErrorOr<Pokemon>> GetPokemonByName(string name);
}
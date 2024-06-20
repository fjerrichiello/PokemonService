using Nvisia.PokemonReview.Api.Persistence.Models;

namespace Nvisia.PokemonReview.Api.Persistence.Repositories;

public interface IPokemonRepository
{
    Task<int> Count();

    Task<bool> PokemonExists(int id, string name);
    
    Task<List<PokemonEntity>> GetAllPokemon(int? page, int? pageSize);

    Task<PokemonEntity> AddPokemon(PokemonEntity entity);
    
    Task<PokemonEntity?> GetPokemonById(int id);
    
    Task<PokemonEntity?> GetPokemonByName(string name);
}
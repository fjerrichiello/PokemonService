using Nvisia.PokemonReview.Api.Clients.Models;

namespace Nvisia.PokemonReview.Api.Clients;

public interface IPokeApiClient
{
   Task<PokeApiResponse?>GetPokemonById(int id);

   Task<PokeApiResponse?>GetPokemonByName(string name);
}
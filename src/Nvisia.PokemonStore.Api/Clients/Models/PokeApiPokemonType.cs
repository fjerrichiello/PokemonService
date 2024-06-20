namespace Nvisia.PokemonReview.Api.Clients.Models;

public class PokeApiPokemonType
{
    public int Slot { get; set; }

    public PokeApiType Type { get; set; } = null!;
}
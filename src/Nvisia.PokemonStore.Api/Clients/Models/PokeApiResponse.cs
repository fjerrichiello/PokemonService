namespace Nvisia.PokemonReview.Api.Clients.Models;

public class PokeApiResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public List<PokeApiPokemonType> Types { get; set; } = [];

    public string Type1 => Types.Count > 0 ? Types.First().Type.Name : "";

    public string? Type2 => Types.Count > 1 ? Types.Last().Type.Name : null;
}
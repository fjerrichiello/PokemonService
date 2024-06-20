namespace Nvisia.PokemonReview.Api.Endpoints.Models;

public class PokemonResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Type1 { get; set; } = null!;

    public string? Type2 { get; set; }
}
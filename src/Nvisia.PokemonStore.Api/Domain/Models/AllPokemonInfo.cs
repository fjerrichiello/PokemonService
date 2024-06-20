namespace Nvisia.PokemonReview.Api.Domain.Models;

public class AllPokemonInfo
{
    public List<Pokemon> Content { get; set; } = [];

    public int Page { get; set; }

    public int PageSize { get; set; }

    public long TotalElements { get; set; }

    public int TotalPages { get; set; }

    public bool Last { get; set; }
}
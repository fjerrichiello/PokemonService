namespace Nvisia.PokemonReview.Api.Endpoints.Models;

public class GetAllPokemonResponse
{
    public List<PokemonResponse> Content { get; set; } = [];

    public int Page { get; set; }

    public int PageSize { get; set; }

    public long TotalElements { get; set; }

    public int TotalPages { get; set; }

    public bool Last { get; set; }
}
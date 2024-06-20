using Microsoft.AspNetCore.Mvc;

namespace Nvisia.PokemonReview.Api.Endpoints.Models;

public class GetAllPokemonRequest
{
    [FromQuery]
    public int? Page { get; set; }

    [FromQuery]
    public int? PageSize { get; set; }
}
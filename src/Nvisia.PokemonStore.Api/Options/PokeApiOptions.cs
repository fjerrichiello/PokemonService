using System.ComponentModel.DataAnnotations;

namespace Nvisia.PokemonReview.Api.Options;

public class PokeApiOptions : OptionsBase
{
    public override string Key => "PokeApi";

    [Required]
    public string BaseUrl { get; set; } = null!;
}
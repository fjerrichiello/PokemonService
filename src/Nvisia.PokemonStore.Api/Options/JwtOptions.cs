using System.ComponentModel.DataAnnotations;

namespace Nvisia.PokemonReview.Api.Options;

public class JwtOptions : OptionsBase
{
    public override string Key => "Jwt";

    [Required]
    public string Issuer { get; set; } = null!;

    [Required]
    public string Audience { get; set; } = null!;

    [Required]
    public string SigningKey { get; set; } = null!;
}
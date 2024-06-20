using System.ComponentModel.DataAnnotations;

namespace Nvisia.PokemonReview.Api.Options;

public class PostgresOptions : OptionsBase
{
    public override string Key => "Postgres";

    [Required]
    public string UserName { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string ServerName { get; set; } = null!;

    [Required]
    public string DatabaseName { get; set; } = null!;

    [Required]
    public string Port { get; set; } = null!;
}
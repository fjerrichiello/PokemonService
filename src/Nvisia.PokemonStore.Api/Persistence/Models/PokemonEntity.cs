using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nvisia.PokemonReview.Api.Persistence.Models;

public class PokemonEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    [MaxLength(25)]
    public string Name { get; set; } = null!;

    [MaxLength(15)]
    public string Type1 { get; set; } = null!;

    [MaxLength(15)]
    public string? Type2 { get; set; }
}
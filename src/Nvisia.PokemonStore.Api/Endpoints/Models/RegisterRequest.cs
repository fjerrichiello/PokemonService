using System.ComponentModel.DataAnnotations;

namespace Nvisia.PokemonReview.Api.Endpoints.Models;

public class RegisterRequest
{
    [Required]
    public string UserName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
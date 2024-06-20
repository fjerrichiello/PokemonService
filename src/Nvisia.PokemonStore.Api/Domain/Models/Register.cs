namespace Nvisia.PokemonReview.Api.Domain.Models;

public class Register
{
    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
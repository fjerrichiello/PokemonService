namespace Nvisia.PokemonReview.Api.Domain.Models;

public abstract class User
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
}
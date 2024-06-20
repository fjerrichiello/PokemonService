namespace Nvisia.PokemonReview.Api.Domain.Models;

public class LoggedInUser : User
{
    public string Token { get; set; } = null!;
}
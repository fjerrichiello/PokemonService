namespace Nvisia.PokemonReview.Api.Endpoints.Models;

public class UserResponse
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
}
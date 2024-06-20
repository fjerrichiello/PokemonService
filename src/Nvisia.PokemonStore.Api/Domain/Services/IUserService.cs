using ErrorOr;
using Nvisia.PokemonReview.Api.Domain.Models;

namespace Nvisia.PokemonReview.Api.Domain.Services;

public interface IUserService
{
    public Task<ErrorOr<LoggedInUser>> Login(Login login);

    public Task<ErrorOr<LoggedInUser>> Register(Register register);
}
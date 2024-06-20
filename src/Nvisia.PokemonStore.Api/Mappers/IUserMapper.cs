using Nvisia.PokemonReview.Api.Domain.Models;
using Nvisia.PokemonReview.Api.Endpoints.Models;
using Nvisia.PokemonReview.Api.Persistence.Models;

namespace Nvisia.PokemonReview.Api.Mappers;

public interface IUserMapper
{
    UserEntity RegisterToUserEntity(Register register);

    Register RegisterRequestToRegister(RegisterRequest registerRequest);

    Login LoginRequestToLogin(LoginRequest loginRequest);
}
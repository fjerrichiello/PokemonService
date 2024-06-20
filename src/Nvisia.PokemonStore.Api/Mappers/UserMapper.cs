using Nvisia.PokemonReview.Api.Domain.Models;
using Nvisia.PokemonReview.Api.Endpoints.Models;
using Nvisia.PokemonReview.Api.Persistence.Models;
using Riok.Mapperly.Abstractions;

namespace Nvisia.PokemonReview.Api.Mappers;

[Mapper]
public partial class UserMapper : IUserMapper
{
    public partial UserEntity RegisterToUserEntity(Register register);

    public partial Register RegisterRequestToRegister(RegisterRequest registerRequest);

    public partial Login LoginRequestToLogin(LoginRequest loginRequest);
}
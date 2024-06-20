using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nvisia.PokemonReview.Api.Domain.Models;
using Nvisia.PokemonReview.Api.Mappers;
using Nvisia.PokemonReview.Api.Options;
using Nvisia.PokemonReview.Api.Persistence.Models;

namespace Nvisia.PokemonReview.Api.Domain.Services;

public class UserService : IUserService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IUserMapper _userMapper;
    private readonly JwtOptions _config;
    private readonly SymmetricSecurityKey _key;

    public UserService(IServiceScopeFactory serviceScopeFactory,
        IOptions<JwtOptions> config, IUserMapper userMapper)
    {
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        _userMapper = userMapper ?? throw new ArgumentNullException(nameof(userMapper));
        _config = config.Value ?? throw new ArgumentNullException(nameof(config));
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.SigningKey));
    }

    public async Task<ErrorOr<LoggedInUser>> Login(Login login)
    {
        var userManager = _serviceScopeFactory.CreateScope().ServiceProvider
            .GetRequiredService<UserManager<UserEntity>>();
        var signInManager = _serviceScopeFactory.CreateScope().ServiceProvider
            .GetRequiredService<SignInManager<UserEntity>>();

        var user = await userManager.Users.FirstOrDefaultAsync(x =>
            x.UserName!.Equals(login.UserName, StringComparison.InvariantCultureIgnoreCase));
        if (user is null)
        {
            return Error.Unauthorized(description: "Invalid UserName");
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, login.Password, false);
        if (!result.Succeeded)
        {
            return Error.Unauthorized(description: "UserName not found and/or password incorrect");
        }

        return new LoggedInUser
        {
            UserName = user.UserName!,
            Email = user.Email!,
            Token = CreateToken(user.Email!, user.UserName!)
        };
    }

    public async Task<ErrorOr<LoggedInUser>> Register(Register register)
    {
        var userManager = _serviceScopeFactory.CreateScope().ServiceProvider
            .GetRequiredService<UserManager<UserEntity>>();

        var user = _userMapper.RegisterToUserEntity(register);

        var createdUser = await userManager.CreateAsync(user, register.Password);
        if (!createdUser.Succeeded)
        {
            return Error.Failure(description: string.Join(", ", createdUser.Errors.ToList()));
        }

        var roleResult = await userManager.AddToRoleAsync(user, "User");
        if (!roleResult.Succeeded)
        {
            return Error.Failure(description: string.Join(", ", roleResult.Errors.ToList()));
        }

        return new LoggedInUser
        {
            UserName = user.UserName!,
            Email = user.Email!,
            Token = CreateToken(user.Email!, user.UserName!)
        };
    }

    private string CreateToken(string email, string userName)
    {
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.GivenName, userName)
        ];

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = credentials,
            Issuer = _config.Issuer,
            Audience = _config.Audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
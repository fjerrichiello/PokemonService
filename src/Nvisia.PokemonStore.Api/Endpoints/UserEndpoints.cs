using FluentValidation;
using Nvisia.PokemonReview.Api.Domain.Services;
using Nvisia.PokemonReview.Api.Endpoints.Models;
using Nvisia.PokemonReview.Api.Mappers;

namespace Nvisia.PokemonReview.Api.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserApi(this RouteGroupBuilder group)
    {
        group.MapPost("login", Login);
        
        group.MapPost("register", Register);

        return group;
    }

    public static async Task<IResult> Login(LoginRequest loginRequest, IUserService userService, IUserMapper userMapper,
        IValidator<LoginRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(loginRequest);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage)));
        }

        var login = userMapper.LoginRequestToLogin(loginRequest);
        var result = await userService.Login(login);

        return result.IsError
            ? Results.Unauthorized()
            : Results.Ok(result.Value);
    }

    public static async Task<IResult> Register(RegisterRequest registerRequest, IUserService userService,
        IUserMapper userMapper, IValidator<RegisterRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(registerRequest);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage)));
        }

        var register = userMapper.RegisterRequestToRegister(registerRequest);
        var result = await userService.Register(register);

        return result.IsError
            ? Results.BadRequest(result.FirstError.Description)
            : Results.Ok(result.Value);
    }
}
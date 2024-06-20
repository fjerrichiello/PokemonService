using FluentValidation;
using Nvisia.PokemonReview.Api.Domain.Services;
using Nvisia.PokemonReview.Api.Endpoints.Models;
using Nvisia.PokemonReview.Api.Mappers;

namespace Nvisia.PokemonReview.Api.Endpoints;

public static class PokemonEndpoints
{
    public static RouteGroupBuilder MapPokemonApi(this RouteGroupBuilder group)
    {
        group.MapGet("", GetAllPokemon);

        group.MapPost("", CreatePokemon);

        group.MapGet("{id:int}", GetPokemonById);

        group.MapGet("{name}", GetPokemonByName);

        return group;
    }

    public static async Task<IResult> GetAllPokemon([AsParameters] GetAllPokemonRequest getAllPokemonRequest,
        IPokemonService pokemonService,
        IPokemonMapper pokemonMapper, IValidator<GetAllPokemonRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(getAllPokemonRequest);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage)));
        }

        var result = await pokemonService.GetAllPokemon(getAllPokemonRequest.Page, getAllPokemonRequest.PageSize);
        return result.IsError
            ? Results.BadRequest()
            : Results.Ok(pokemonMapper.AllPokemonInfoToGetAllPokemonResponse(result.Value));
    }

    public static async Task<IResult> GetPokemonById(int id, IPokemonService pokemonService,
        IPokemonMapper pokemonMapper)
    {
        if (id < 1)
            return Results.BadRequest("Invalid Id");

        var result = await pokemonService.GetPokemonById(id);
        return result.IsError
            ? Results.BadRequest(result.FirstError.Description)
            : Results.Ok(pokemonMapper.PokemonToPokemonResponse(result.Value));
    }

    public static async Task<IResult> GetPokemonByName(string name, IPokemonService pokemonService,
        IPokemonMapper pokemonMapper)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Results.BadRequest("Invalid Name");

        var result = await pokemonService.GetPokemonByName(name);
        return result.IsError
            ? Results.BadRequest(result.FirstError.Description)
            : Results.Ok(pokemonMapper.PokemonToGetPokemonResponse(result.Value));
    }

    public static async Task<IResult> CreatePokemon(CreatePokemonRequest createPokemonRequest,
        IPokemonService pokemonService, IPokemonMapper pokemonMapper, IValidator<CreatePokemonRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(createPokemonRequest);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage)));
        }

        var pokemon = pokemonMapper.CreatePokemonRequestToPokemon(createPokemonRequest);
        var result = await pokemonService.CreatePokemon(pokemon);
        return result.IsError
            ? Results.BadRequest(result.FirstError.Description)
            : Results.Accepted(value: pokemonMapper.PokemonToCreatePokemonResponse(result.Value));
    }
}
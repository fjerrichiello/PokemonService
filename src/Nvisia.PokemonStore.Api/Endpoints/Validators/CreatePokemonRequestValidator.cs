using FluentValidation;
using Nvisia.PokemonReview.Api.Endpoints.Models;

namespace Nvisia.PokemonReview.Api.Endpoints.Validators;

public class CreatePokemonRequestValidator : AbstractValidator<CreatePokemonRequest>
{
    public CreatePokemonRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Type1)
            .NotEmpty();
    }
}
using FluentValidation;
using Nvisia.PokemonReview.Api.Endpoints.Models;

namespace Nvisia.PokemonReview.Api.Endpoints.Validators;

public class GetAllPokemonRequestValidator : AbstractValidator<GetAllPokemonRequest>
{
    public GetAllPokemonRequestValidator()
    {
        When(x => x.Page is not null, () =>
        {
            RuleFor(y => y.Page)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .NotNull()
                .GreaterThan(0);
        }).Otherwise(() =>
        {
            RuleFor(y => y.PageSize)
                .Null();
        });
    }
}
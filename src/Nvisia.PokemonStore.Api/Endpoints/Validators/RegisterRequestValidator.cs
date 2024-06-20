using FluentValidation;
using Nvisia.PokemonReview.Api.Endpoints.Models;

namespace Nvisia.PokemonReview.Api.Endpoints.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.UserName)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
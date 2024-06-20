using FluentValidation;
using Nvisia.PokemonReview.Api.Endpoints.Models;

namespace Nvisia.PokemonReview.Api.Endpoints.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
using System.ComponentModel.DataAnnotations;

namespace Nvisia.PokemonReview.Api.Options;

public abstract class OptionsBase
{
    public abstract string Key { get; }

    public virtual void Validate()
    {
        Validate(this);
    }

    private static void Validate(object obj) =>
        Validator.ValidateObject(
            obj,
            new ValidationContext(obj),
            validateAllProperties: true);
}
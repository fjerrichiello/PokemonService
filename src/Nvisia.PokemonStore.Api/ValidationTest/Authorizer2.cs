using FluentValidation;

namespace Nvisia.PokemonReview.Api.ValidationTest;

public class Authorizer2 : AbstractValidator<ValidParams>
{
    public Authorizer2()
    {
        RuleSet("IsMemberEffective",
            () => { RuleFor(x => x.member.EffectiveDate).LessThanOrEqualTo(x => x.member.EffectiveDate); });

        RuleSet("IsMemberIneffective",
            () => { RuleFor(x => x.member.EffectiveDate).GreaterThan(x => x.member.EffectiveDate); });

        RuleSet("IsMember", () => { RuleFor(x => x.member.MemberType).Must(MemberTypes.IsMemberType); });

        RuleSet("IsMemberServicer",
            () =>
            {
                RuleFor(x => x.member.MemberType).Must(x => x.HasValue && MemberTypes.IsMemberServicer(x.Value));
            });

        RuleSet("HasOneAndTwoSigned", () =>
        {
            RuleFor(x => x.member.SignedOne).Equals(true);
            RuleFor(x => x.member.SignedTwo).Equals(true);
        });

        RuleSet("HasThreeSigned", () => { RuleFor(x => x.member.SignedThree).Equals(true); });

        RuleSet("HasAnyRole",
            () => { RuleFor(x => x.Roles).Must(x => x.Any(y => HasInternalRole(y) || HasExternalRole(y))); });

        RuleSet("HasExternalRole",
            () => { RuleFor(x => x.Roles).Must(x => x.Any(HasExternalRole)); });

        RuleSet("HasInternalRole",
            () => { RuleFor(x => x.Roles).Must(x => x.Any(HasInternalRole)); });
    }

    private static bool HasInternalRole(string role)
    {
        var list = new List<string> { "test" };
        return list.Contains(role);
    }

    private static bool HasExternalRole(string role)
    {
        var list = new List<string> { "test2" };
        return list.Contains(role);
    }
}
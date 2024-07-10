using FluentValidation;

namespace Nvisia.PokemonReview.Api.ValidationTest;

public class Authorizer : AbstractValidator<ValidParams>
{
    public Authorizer()
    {
        RuleSet("HasEffectivePermissions", () =>
        {
            RuleFor(x => x.member.MemberType).Must(MemberTypes.IsMemberType);
            RuleFor(x => x.member.EffectiveDate).LessThanOrEqualTo(x => x.member.EffectiveDate);
            RuleFor(x => x.member.SignedOne).Equals(true);
            RuleFor(x => x.member.SignedTwo).Equals(true);
            RuleFor(x => x.Roles).Must(x => x.Any(y => HasInternalRole(y) || HasExternalRole(y)));
        });

        RuleSet("HasNonEffectivePermissions", () =>
        {
            RuleFor(x => x.member.MemberType).Must(MemberTypes.IsMemberType);
            RuleFor(x => x.member.EffectiveDate).GreaterThan(x => x.member.EffectiveDate);
            RuleFor(x => x.member.SignedOne).Equals(true);
            RuleFor(x => x.member.SignedTwo).Equals(true);
            RuleFor(x => x.Roles).Must(x => x.Any(HasInternalRole));
        });

        RuleSet("HasEffectiveNonMemberServicerCustodialPermissions", () =>
        {
            RuleFor(x => x.member.MemberType).Must(x => x.HasValue && MemberTypes.IsMemberServicer(x.Value));
            RuleFor(x => x.member.EffectiveDate).LessThanOrEqualTo(x => x.member.EffectiveDate);
            RuleFor(x => x.member.SignedThree).Equals(true);
            RuleFor(x => x.Roles).Must(x => x.Any(HasExternalRole));
        });
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
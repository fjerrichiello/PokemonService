using System.Collections.Immutable;

namespace Nvisia.PokemonReview.Api.ValidationTest;

public interface IMember
{
    public MemberType? MemberType { get; set; }

    public bool SignedOne { get; set; }
    public bool SignedTwo { get; set; }
    public bool SignedThree { get; set; }

    DateOnly? EffectiveDate { get; set; }
}

public class Member : IMember
{
    public Guid Id { get; set; }
    public MemberType? MemberType { get; set; }
    public bool SignedOne { get; set; }
    public bool SignedTwo { get; set; }
    public bool SignedThree { get; set; }
    public DateOnly? EffectiveDate { get; set; }
}

public enum MemberType
{
    Member,
    Non7th,
    Servicer
}

public static class MemberTypes
{
    private static IEnumerable<MemberType>
        MemberServicer = ImmutableList.Create(MemberType.Non7th, MemberType.Servicer);

    public static bool IsMemberType(MemberType? memberType) => MemberType.Member.Equals(memberType);

    public static bool IsMemberServicer(MemberType memberType) => MemberServicer.Contains(memberType);
}
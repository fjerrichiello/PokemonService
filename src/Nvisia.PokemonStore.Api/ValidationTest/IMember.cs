using System.Collections.Immutable;

namespace Nvisia.PokemonReview.Api.ValidationTest;

public interface IMember
{
    public MemberType? MemberType { get; init; }

    public bool SignedOne { get; init; }
    public bool SignedTwo { get; init; }
    public bool SignedThree { get; init; }

    DateOnly? EffectiveDate { get; init; }
}

public record Member(
    Guid Id,
    MemberType? MemberType,
    bool SignedOne,
    bool SignedTwo,
    bool SignedThree,
    DateOnly? EffectiveDate) : IMember;

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
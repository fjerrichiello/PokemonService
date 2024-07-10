using Dumpify;
using FluentValidation;
using Nvisia.PokemonReview.Api.ValidationTest;
using Xunit.Abstractions;

namespace Nvisia.PokemonReview.UnitTests.TestValidation;

public class AuthorizerTest
{
    private static readonly Authorizer _authorizer = new Authorizer();
    private static readonly Authorizer2 _authorizer2 = new Authorizer2();

    private readonly ITestOutputHelper _output;


    public AuthorizerTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Test1()
    {
        var member = new Member();
        member.MemberType = MemberType.Member;
        var test = new ValidParams(member, ["test"], new List<string>(),
            DateOnly.FromDateTime(DateTime.UtcNow));
        var result = Authorizer1(test);
        _output.WriteLine(result.DumpText());

        var result2 = Authorizer2(test);
        _output.WriteLine(result2.DumpText());
    }

    private bool Authorizer1(ValidParams test)
    {
        List<string> RuleSets =
        [
            "HasEffectivePermissions", "HasNonEffectivePermissions", "HasEffectiveNonMemberServicerCustodialPermissions"
        ];

        return RuleSets.Select(x =>
        {
            var result = _authorizer.Validate(test, options => options.IncludeRuleSets(x));
            _output.WriteLine($"{x} {result.IsValid.DumpText()}");
            return result.IsValid;
        }).Aggregate(false, (acc, b) => acc || b);
    }

    private bool Authorizer2(ValidParams test)
    {
        List<(
            string RuleName, List<string> Rules
            )> RuleSets =
        [
            new("HasEffectivePermissions", ["IsMemberEffective", "IsMember", "HasOneAndTwoSigned", "HasAnyRole"]),
            new("HasNonEffectivePermissions",
                ["IsMemberIneffective", "IsMember", "HasOneAndTwoSigned", "HasInternalRole"]),
            new("HasEffectiveNonMemberServicerCustodialPermissions",
                ["IsMemberEffective", "IsMember", "HasThreeSigned", "HasExternalRole"])
        ];

        return RuleSets.Select(x =>
        {
            var ruleSetResult = x.Rules.Select(y =>
            {
                var result = _authorizer2.Validate(test, options => options.IncludeRuleSets(y));
                _output.WriteLine($"{y} {result.IsValid.DumpText()}");
                return result.IsValid;
            }).Aggregate(true, (acc, b) => acc && b);
            _output.WriteLine($"{x.RuleName} {ruleSetResult.DumpText()}");
            return ruleSetResult;
        }).Aggregate(false, (acc, b) => acc || b);
    }
}
namespace Nvisia.PokemonReview.Api.ValidationTest;

public record ValidParams(IMember member, List<string> Roles, List<string> OtherRoles, DateOnly date);

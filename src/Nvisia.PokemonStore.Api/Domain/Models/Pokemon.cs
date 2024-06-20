using System.Diagnostics.CodeAnalysis;

namespace Nvisia.PokemonReview.Api.Domain.Models;

public class Pokemon
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Type1 { get; set; } = null!;

    public string? Type2 { get; set; }


    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        return Equals(obj as Pokemon);
    }

    protected bool Equals(Pokemon? other)
    {
        if (other == null) return false;

        // Compare each property, ignoring case for strings
        return Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase)
               && Type1.Equals(other.Type1, StringComparison.OrdinalIgnoreCase)
               && (Type2 ?? "").Equals(other.Type2 ?? "", StringComparison.OrdinalIgnoreCase);
    }

    
    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode()
    {
        // Combine hash codes of properties
        return HashCode.Combine(Id, Name, Type1, Type2);
    }
}
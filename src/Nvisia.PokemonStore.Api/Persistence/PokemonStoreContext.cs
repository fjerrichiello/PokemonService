using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nvisia.PokemonReview.Api.Persistence.Models;

namespace Nvisia.PokemonReview.Api.Persistence;

public class PokemonStoreContext : IdentityDbContext<UserEntity>
{
    public DbSet<PokemonEntity> PokemonEntities { get; set; }

    public PokemonStoreContext(
        DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        List<IdentityRole> roles =
        [
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER"
            },
        ];

        modelBuilder.Entity<IdentityRole>().HasData(roles);
    }
}
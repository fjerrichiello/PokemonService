using Microsoft.EntityFrameworkCore;
using Nvisia.PokemonReview.Api.Persistence.Models;

namespace Nvisia.PokemonReview.Api.Persistence.Repositories;

public class PokemonRepository : IPokemonRepository
{
    private readonly IDbContextFactory<PokemonStoreContext> _contextFactory;

    public PokemonRepository(IDbContextFactory<PokemonStoreContext> contextFactory)
    {
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
    }

    public async Task<int> Count()
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();

        return await dbContext.PokemonEntities.CountAsync();
    }

    public async Task<bool> PokemonExists(int id, string name)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();

        return await dbContext.PokemonEntities.AnyAsync(x =>
            x.Id == id || x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<List<PokemonEntity>> GetAllPokemon(int? page, int? pageSize)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();

        if (page is null || pageSize is null)
        {
            return await dbContext.PokemonEntities.ToListAsync();
        }

        return await dbContext.PokemonEntities
            .OrderBy(x => x.Id)
            .Skip(page.Value * pageSize.Value)
            .Take(pageSize.Value)
            .ToListAsync();
    }

    public async Task<PokemonEntity> AddPokemon(PokemonEntity entity)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        var result = await dbContext.AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<PokemonEntity?> GetPokemonById(int id)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        return await dbContext.PokemonEntities.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PokemonEntity?> GetPokemonByName(string name)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        return await dbContext.PokemonEntities.FirstOrDefaultAsync(x => x.Name == name);
    }
}
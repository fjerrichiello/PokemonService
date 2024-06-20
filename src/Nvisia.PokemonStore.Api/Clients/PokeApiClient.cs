using Nvisia.PokemonReview.Api.Clients.Models;
using Nvisia.PokemonReview.Api.Options;

namespace Nvisia.PokemonReview.Api.Clients;

public class PokeApiClient : IPokeApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    private const string GetPokemonPath = "pokemon/{0}";

    public PokeApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public async Task<PokeApiResponse?> GetPokemonById(int id)
    {
        var requestPath = string.Format(GetPokemonPath, id);
        var response = await Get(requestPath);
        if (response is null) throw new Exception();
        return response;
    }

    public async Task<PokeApiResponse?> GetPokemonByName(string name)
    {
        var requestPath = string.Format(GetPokemonPath, name);
        var response = await Get(requestPath);
        if (response is null) throw new Exception();
        return response;
    }


    private async Task<PokeApiResponse?> Get(string route) =>
        await CreateClient().GetFromJsonAsync<PokeApiResponse>(route);

    private HttpClient CreateClient() => _httpClientFactory.CreateClient(new PokeApiOptions().Key);
}
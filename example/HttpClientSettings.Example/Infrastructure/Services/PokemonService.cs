using Microsoft.Extensions.Options;

namespace HttpClientSettings.Example.Infrastructure;

public class PokemonService : IPokemonService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClientAppSettings _httpClientOptions;

    public PokemonService(IHttpClientFactory httpClientFactory,
        IOptions<HttpClientAppSettings> httpClientOptions)
    {
        _httpClientFactory = httpClientFactory;
        _httpClientOptions = httpClientOptions.Value;
    }

    public async Task<PagedPokemon> GetPagedPokemon(int take = 10, int skip = 0, CancellationToken cancellationToken = default)
    {
        var client = _httpClientFactory.CreateClient();

        var endpoint = _httpClientOptions.GetEndpoint("Pokemon", "GetPagedPokemon", skip, take);

        return await client.GetFromJsonAsync<PagedPokemon>(endpoint.FullUri, cancellationToken)
            ?? new PagedPokemon();
    }

    public async Task<Pokemon> GetPokemon(int id, CancellationToken cancellationToken = default)
    {
        var client = _httpClientFactory.CreateClient();

        var endpoint = _httpClientOptions.GetEndpoint("Pokemon", "GetPokemon", id);        

        return await client.GetFromJsonAsync<Pokemon>(endpoint.FullUri, cancellationToken)
            ?? new Pokemon();
    }
}

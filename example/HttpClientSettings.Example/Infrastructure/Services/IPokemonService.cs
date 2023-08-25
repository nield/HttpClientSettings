namespace HttpClientSettings.Example.Infrastructure;

public interface IPokemonService
{
    Task<PagedPokemon> GetPagedPokemon(int take = 10, int skip = 0, CancellationToken cancellationToken = default);

    Task<Pokemon> GetPokemon(int id, CancellationToken cancellationToken = default);
}

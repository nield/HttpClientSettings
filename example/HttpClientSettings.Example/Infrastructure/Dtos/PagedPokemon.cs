namespace HttpClientSettings.Example.Infrastructure;

public class PagedPokemon
{
    public int Count { get; set; }

    public string Next { get; set; } = "";

    public string Previous { get; set; } = "";

    public List<PagedPokemonDto> Results { get; set; } = new List<PagedPokemonDto>();
}

public class PagedPokemonDto
{
    public string Url { get; set; } = "";
    public string Name { get; set; } = "";
}

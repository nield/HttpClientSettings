namespace HttpClientSettings.Example.Models.Responses;

public class PagedPokemonResponse
{
    public int Count { get; set; }

    public List<PagedPokemonResponseDto> Results { get; set; } = new List<PagedPokemonResponseDto>();
}

public class PagedPokemonResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

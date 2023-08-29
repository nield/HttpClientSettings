# HttpClientSettings
[![.NET](https://github.com/nield/HttpClientSettings/actions/workflows/dotnet.yml/badge.svg)](https://github.com/nield/HttpClientSettings/actions/workflows/dotnet.yml)

HttpClientSettings is a convention based http endpoint data storage and retrieval mechanism without enforcing how to make the http calls. It is up to the developer what tech the want to use to make the http calls, this is purely for storing and retrieving endpoint data. 

The data structure stored in appSettings allow for storing multiple endpoints grouped under a client.

This library removes the pain of coming up with your own storage structure and boilerplate code retrieving endpoint urls, validation on data and replacing parameters in the url.

## Usage

In order to use the *IOptions<HttpClientAppSettings>* to retrieve the stored endpoints the library must be added to the program

```csharp
builder.Services.AddHttpClientSettings(builder.Configuration);
```

In the appSettings.json file add a new section called **HttpClientSettings** with your endpoints grouped under a client. Please ensure each Client Name is unique as well as the endpoint name under each client.

Passing parameters to the url is achieved by specifying the placeholders in the url with an index as you would with normal string.Format in C#

```json
"HttpClientSettings": {
    "Clients": [
      {
        "Name": "Pokemon",
        "BaseUri": "https://pokeapi.co",
        "Endpoints": [
          {
            "Uri": "api/v2/pokemon?offset={0}&limit={1}",
            "Name": "GetPagedPokemon"
          },
          {
            "Uri": "api/v2/pokemon/{0}",
            "Name": "GetPokemon"
          }
        ]
      }
    ]
  }
```

In order to use the stored settings you use dependency injection to get the *IOptions<HttpClientAppSettings>*

Below is an example service. To have a better understanding you can view and run the example project **HttpClientSettings.Example** in source code

```csharp
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
```


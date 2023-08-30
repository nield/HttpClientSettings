using AutoMapper;
using HttpClientSettings;
using HttpClientSettings.Example.Infrastructure;
using HttpClientSettings.Example.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddHttpClientSettings(builder.Configuration, validateSettings: true);

builder.Services.AddScoped<IPokemonService, PokemonService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/pokemon", async (
    [FromQuery]int take,
    [FromQuery]int skip,
    [FromServices] IPokemonService pokemonService,
    IMapper mapper,
    CancellationToken cancellationToken) =>
{
    var data = await pokemonService.GetPagedPokemon(take, skip, cancellationToken);

    var mappedData = mapper.Map<PagedPokemonResponse>(data);

    return Results.Ok(mappedData);
})
.WithName("GetAllPokemon")
.WithOpenApi();

app.MapGet("/pokemon/{id}", async (
    [FromRoute] int id,
    [FromServices] IPokemonService pokemonService,
    IMapper mapper,
    CancellationToken cancellationToken) =>
{
    var data = await pokemonService.GetPokemon(id, cancellationToken);

    var mappedData = mapper.Map<PokemonResponse>(data);

    return Results.Ok(mappedData);
})
.WithName("GetPokemon")
.WithOpenApi();

app.Run();

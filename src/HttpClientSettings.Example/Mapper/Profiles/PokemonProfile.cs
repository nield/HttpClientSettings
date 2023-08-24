using AutoMapper;
using HttpClientSettings.Example.Infrastructure;
using HttpClientSettings.Example.Mapper.Resolvers;
using HttpClientSettings.Example.Models.Responses;

namespace HttpClientSettings.Example.Mapper.Profiles;

public class PokemonProfile : Profile
{   
    public PokemonProfile()
    {
        CreateMap<PagedPokemonDto, PagedPokemonResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom<PokemonIdResolver>());

        CreateMap<PagedPokemon, PagedPokemonResponse>();

        CreateMap<Pokemon, PokemonResponse>();
    }
}

using AutoMapper;
using HttpClientSettings.Example.Infrastructure;
using HttpClientSettings.Example.Models.Responses;
using System.Text.RegularExpressions;

namespace HttpClientSettings.Example.Mapper.Resolvers;

public class PokemonIdResolver : IValueResolver<PagedPokemonDto, PagedPokemonResponseDto, int>
{
    private static readonly Regex _getNumberRegex = new Regex("(\\d+)(?!.*\\d)", RegexOptions.Compiled);

    public int Resolve(PagedPokemonDto source, 
        PagedPokemonResponseDto destination, 
        int destMember, 
        ResolutionContext context)
    {
        var match = _getNumberRegex.Match(source.Url);

        if (match.Success && int.TryParse(match.Value, out int result))
        {
            return result;
        }

        return 0;
    }
}

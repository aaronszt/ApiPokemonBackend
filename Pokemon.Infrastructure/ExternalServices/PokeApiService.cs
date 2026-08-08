using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using Pokemon.Domain.QueryModel.Dtos;
using Pokemon.Domain.Interfaces;

namespace Pokemon.Infrastructure.ExternalServices;

public class PokeApiService : IPokeApiService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private const string PokemonCacheKey = "PokemonList";

    public PokeApiService(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<IEnumerable<PokemonsDto>> GetPokemonsFromApiAsync()
    {
        if (_cache.TryGetValue(PokemonCacheKey, out IEnumerable<PokemonsDto>? cachedPokemons))
        {
            return cachedPokemons!;
        }

        var response = await _httpClient.GetFromJsonAsync<PokeApiResponse>("pokemon?limit=100"); 
        if (response == null) return Enumerable.Empty<PokemonsDto>();       

        var tasks = response.Results.Select(async r =>
        {
            var detail = await _httpClient.GetFromJsonAsync<PokeApiDetailResponse>(r.Url);
            return new PokemonsDto
            {
            Id = detail!.Id,
            Name = detail.Name,
            Image = detail.Sprites.Front_Default,
            Hp = detail.Stats.FirstOrDefault(s => s.Stat.Name == "hp")?.Base_Stat ?? 0,
            Attack = detail.Stats.FirstOrDefault(s => s.Stat.Name == "attack")?.Base_Stat ?? 0,
            Defense = detail.Stats.FirstOrDefault(s => s.Stat.Name == "defense")?.Base_Stat ?? 0,
            Speed = detail.Stats.FirstOrDefault(s => s.Stat.Name == "speed")?.Base_Stat ?? 0,
            Height = detail.Height,
            Weight = detail.Weight,
            Types = detail.Types.Select(t => t.Type.Name).ToList(),
            Custom = false
            };
        });

        var pokemons = await Task.WhenAll(tasks);

        var cacheOptions = new MemoryCacheEntryOptions()
        .SetAbsoluteExpiration(TimeSpan.FromHours(24));
        _cache.Set(PokemonCacheKey, pokemons, cacheOptions);
        
        return pokemons;
    }
}

public record PokeApiResponse(List<PokeApiResult> Results);
public record PokeApiResult(string Name, string Url);
public record PokeApiDetailResponse(
    int Id,
    string Name,
    List<PokeApiStat> Stats,
    PokeApiSprites Sprites,
    List<PokeApiTypeSlot> Types,
    int Height,
    int Weight
);
public record PokeApiStat(int Base_Stat, PokeApiStatInfo Stat);
public record PokeApiStatInfo(string Name);
public record PokeApiSprites(string Front_Default);
public record PokeApiTypeSlot(PokeApiTypeInfo Type);
public record PokeApiTypeInfo(string Name);
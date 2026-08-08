using Pokemon.Domain.QueryModel.Dtos;

namespace Pokemon.Domain.Interfaces;

public interface IPokeApiService
{
    Task<IEnumerable<PokemonsDto>> GetPokemonsFromApiAsync();
}
using Pokemon.Domain.QueryModel.Dtos;

namespace Pokemon.Domain.QueryModel.Repositories
{
    public interface IPokemonsDtoRepository
    {
        IEnumerable<PokemonsDto> GetAllPokemons();
        PokemonsDto? GetPokemonById(int id);
        PokemonsDto? GetPokemonByName(string name);
    }
}
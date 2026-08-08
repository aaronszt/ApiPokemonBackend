using Pokemon.Domain.Entities;

namespace Pokemon.Domain.Interfaces
{
    public interface IPokemonRepository
    {
        Task AddAsync(CreatePokemon pokemon);
        Task DeleteAsync(int id);
    }
}
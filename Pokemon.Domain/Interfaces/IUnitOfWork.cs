using Pokemon.Domain.QueryModel.Repositories;

namespace Pokemon.Domain.Interfaces;

public interface IUnitOfWork
{
    IPokemonsDtoRepository PokemonsDtoRepository { get;}
    IPokemonRepository PokemonsRepository { get; }
    IPokemonTypeRepository PokemonTypeRepository { get; }
    Task<int> SaveChangesAsync();
}
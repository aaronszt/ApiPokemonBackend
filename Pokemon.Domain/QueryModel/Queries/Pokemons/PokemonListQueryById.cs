using Pokemon.Domain.QueryModel.Dtos;
using MediatR;

namespace Pokemon.Domain.QueryModel.Queries.Pokemons
{
    public class PokemonListQueryById : IRequest<PokemonsDto>
    {
        public int Id { get; set; }

        public PokemonListQueryById(int id)
        {
            Id = id;
        }
    }
}
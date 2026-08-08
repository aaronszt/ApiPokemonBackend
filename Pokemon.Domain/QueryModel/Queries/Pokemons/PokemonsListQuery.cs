using MediatR;
using Pokemon.Domain.QueryModel.Dtos;

namespace Pokemon.Domain.QueryModel.Queries.Pokemons
{
    public class PokemonsListQuery : IRequest<IEnumerable<PokemonsDto>>
    {
    }
}
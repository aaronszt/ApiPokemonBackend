using Pokemon.Domain.QueryModel.Dtos;
using MediatR;

namespace Pokemon.Domain.QueryModel.Queries.Types
{
    public class TypesListQuery : IRequest<IEnumerable<PokemonTypeDto>>
    {
    }
}
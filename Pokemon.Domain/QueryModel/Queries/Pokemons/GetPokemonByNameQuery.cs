using Pokemon.Domain.QueryModel.Dtos;
using MediatR;

namespace Pokemon.Domain.QueryModel.Queries.Pokemons
{
    public class GetPokemonByNameQuery : IRequest<PokemonsDto>
    {
        public string Name { get; set;}

        public GetPokemonByNameQuery(string name)
        {
            Name = name;
        }
    }
}
using Pokemon.Domain.Entities;
using MediatR;

namespace Pokemon.Domain.QueryModel.Commands.Pokemons
{
    public class CreatePokemonCommand : IRequest<CreatePokemon>
    {
        public CreatePokemon Pokemon { get; }

        public CreatePokemonCommand(CreatePokemon pokemon)
        {
            Pokemon = pokemon;
        }
    }
}
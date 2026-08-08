using MediatR;

namespace Pokemon.Domain.QueryModel.Commands.Pokemons
{
    public class DeletePokemonCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        public DeletePokemonCommand(int id)
        {
            Id = id;
        }
    }
}
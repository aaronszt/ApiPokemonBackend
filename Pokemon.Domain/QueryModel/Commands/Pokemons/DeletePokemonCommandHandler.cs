using Pokemon.Domain.Interfaces;
using Pokemon.Domain.Exceptions;
using MediatR;

namespace Pokemon.Domain.QueryModel.Commands.Pokemons
{
    public class DeletePokemonCommandHandler : IRequestHandler<DeletePokemonCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePokemonCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeletePokemonCommand request, CancellationToken cancellationToken)
        {
            var pokemonExist = _unitOfWork.PokemonsDtoRepository.GetPokemonById(request.Id);
            if (pokemonExist == null) throw new NotFoundException($"No se pudo encontrar al pokemon con ID: {request.Id}");

            await _unitOfWork.PokemonsRepository.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
using MediatR;
using Pokemon.Domain.Exceptions;
using Pokemon.Domain.Interfaces;
using Pokemon.Domain.QueryModel.Dtos;

namespace Pokemon.Domain.QueryModel.Queries.Pokemons
{
    public class PokemonListQueryByIdHandler : IRequestHandler<PokemonListQueryById, PokemonsDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PokemonListQueryByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<PokemonsDto> Handle(PokemonListQueryById request, CancellationToken cancellationToken)
        {
            var pokemon = _unitOfWork.PokemonsDtoRepository.GetPokemonById(request.Id);
            if (pokemon == null) throw new NotFoundException($"No se pudo encontrar al pokemon con ID: {request.Id}");
            return Task.FromResult(pokemon);
        }
    }
}
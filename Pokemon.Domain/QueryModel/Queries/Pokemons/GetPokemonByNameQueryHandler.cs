using Pokemon.Domain.QueryModel.Dtos;
using Pokemon.Domain.Exceptions;
using Pokemon.Domain.Interfaces;
using MediatR;

namespace Pokemon.Domain.QueryModel.Queries.Pokemons
{
    public class GetPokemonByNameQueryHandler : IRequestHandler<GetPokemonByNameQuery, PokemonsDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPokemonByNameQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<PokemonsDto> Handle(GetPokemonByNameQuery request, CancellationToken cancellationToken)
        {
            var pokemon = _unitOfWork.PokemonsDtoRepository.GetPokemonByName(request.Name);
            if (pokemon == null) throw new NotFoundException($"No se pudo encontrar al pokemon con Name: {request.Name}");
            return Task.FromResult(pokemon);
        }
    }
}
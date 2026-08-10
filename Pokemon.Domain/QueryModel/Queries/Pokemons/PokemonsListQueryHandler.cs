using Pokemon.Domain.QueryModel.Dtos;
using Pokemon.Domain.Interfaces;
using MediatR;

namespace Pokemon.Domain.QueryModel.Queries.Pokemons
{
    public class PokemonsListQueryHandler : IRequestHandler<PokemonsListQuery, IEnumerable<PokemonsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PokemonsListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<PokemonsDto>> Handle(PokemonsListQuery request, CancellationToken cancellationToken)
        {
            var pokemons = _unitOfWork.PokemonsDtoRepository.GetAllPokemons();
            return Task.FromResult(pokemons);
        }
    }
}
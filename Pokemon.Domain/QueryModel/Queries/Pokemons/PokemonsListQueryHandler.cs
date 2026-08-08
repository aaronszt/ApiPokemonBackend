using Pokemon.Domain.QueryModel.Dtos;
using Pokemon.Domain.Interfaces;
using MediatR;

namespace Pokemon.Domain.QueryModel.Queries.Pokemons
{
    public class PokemonsListQueryHandler : IRequestHandler<PokemonsListQuery, IEnumerable<PokemonsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPokeApiService _pokeApiService;

        public PokemonsListQueryHandler(IUnitOfWork unitOfWork, IPokeApiService pokeApiService)
        {
            _unitOfWork = unitOfWork;
            _pokeApiService = pokeApiService;
        }

        public async Task<IEnumerable<PokemonsDto>> Handle(PokemonsListQuery request, CancellationToken cancellationToken)
        {
            var dbPokemons = _unitOfWork.PokemonsDtoRepository.GetAllPokemons();
            var apiPokemons = await _pokeApiService.GetPokemonsFromApiAsync();
            return dbPokemons.Concat(apiPokemons);
        }
    }
}
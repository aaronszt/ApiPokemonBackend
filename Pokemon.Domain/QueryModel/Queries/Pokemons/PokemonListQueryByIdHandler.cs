using MediatR;
using Pokemon.Domain.Exceptions;
using Pokemon.Domain.Interfaces;
using Pokemon.Domain.QueryModel.Dtos;

namespace Pokemon.Domain.QueryModel.Queries.Pokemons
{
    public class PokemonListQueryByIdHandler : IRequestHandler<PokemonListQueryById, PokemonsDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPokeApiService _pokeApiService;

        public PokemonListQueryByIdHandler(IUnitOfWork unitOfWork, IPokeApiService pokeApiService)
        {
            _unitOfWork = unitOfWork;
            _pokeApiService = pokeApiService;
        }

        public async Task<PokemonsDto> Handle(PokemonListQueryById request, CancellationToken cancellationToken)
        {
            var dbPokemon = _unitOfWork.PokemonsDtoRepository.GetPokemonById(request.Id);
            if (dbPokemon != null) return dbPokemon;

            var apiPokemon = await _pokeApiService.GetPokemonsFromApiAsync();
            var pokemon = apiPokemon.FirstOrDefault(p => p.Id == request.Id);
            if (pokemon == null) throw new NotFoundException($"No se pudo encontrar al pokemon con ID: {request.Id}");

            return pokemon;
        }
    }
}
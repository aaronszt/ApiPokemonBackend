using Pokemon.Domain.QueryModel.Dtos;
using Pokemon.Domain.Exceptions;
using Pokemon.Domain.Interfaces;
using MediatR;

namespace Pokemon.Domain.QueryModel.Queries.Pokemons
{
    public class GetPokemonByNameQueryHandler : IRequestHandler<GetPokemonByNameQuery, PokemonsDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPokeApiService _pokeApiService;

        public GetPokemonByNameQueryHandler(IUnitOfWork unitOfWork, IPokeApiService pokeApiService)
        {
            _unitOfWork = unitOfWork;
            _pokeApiService = pokeApiService;
        }

        public async Task<PokemonsDto> Handle(GetPokemonByNameQuery request, CancellationToken cancellationToken)
        {
            var pokemonDb = _unitOfWork.PokemonsDtoRepository.GetPokemonByName(request.Name);
            if (pokemonDb != null) return pokemonDb;

            var apiPokemon = await _pokeApiService.GetPokemonsFromApiAsync();
            var pokemon = apiPokemon.FirstOrDefault(p => p.Name?.Equals(request.Name, StringComparison.OrdinalIgnoreCase) ?? false);
            if (pokemon == null) throw new NotFoundException($"No se pudo encontrar al pokemon con Name: {request.Name}");

            return pokemon;
        }
    }
}
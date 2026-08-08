using Pokemon.Domain.Interfaces;
using Pokemon.Domain.Entities;
using MediatR;

namespace Pokemon.Domain.QueryModel.Commands.Pokemons
{
    public class CreatePokemonCommandHandler : IRequestHandler<CreatePokemonCommand, CreatePokemon>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePokemonCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreatePokemon> Handle(CreatePokemonCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.PokemonsRepository.AddAsync(request.Pokemon);
            await _unitOfWork.SaveChangesAsync();

            return request.Pokemon;
        }
    }
}
using Pokemon.Domain.QueryModel.Dtos;
using Pokemon.Domain.Interfaces;
using MediatR;

namespace Pokemon.Domain.QueryModel.Queries.Types
{
    public class TypesListQueryHandler : IRequestHandler<TypesListQuery, IEnumerable<PokemonTypeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TypesListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PokemonTypeDto>> Handle(TypesListQuery request, CancellationToken cancellationToken)
        {
            return _unitOfWork.PokemonTypeRepository.GetAllTypes();
        }
    }
}
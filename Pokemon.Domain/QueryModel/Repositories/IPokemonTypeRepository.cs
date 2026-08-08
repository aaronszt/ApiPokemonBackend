using Pokemon.Domain.QueryModel.Dtos;

namespace Pokemon.Domain.QueryModel.Repositories
{
    public interface IPokemonTypeRepository
    {
        IEnumerable<PokemonTypeDto> GetAllTypes();
    }
}
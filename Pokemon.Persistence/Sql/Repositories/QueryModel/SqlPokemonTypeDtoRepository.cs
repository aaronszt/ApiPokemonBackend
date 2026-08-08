using Pokemon.Domain.QueryModel.Repositories;
using Pokemon.Domain.QueryModel.Dtos;

namespace Pokemon.Persistence.Sql.Repositories.QueryModel
{
    public class SqlPokemonTypeDtoRepository : IPokemonTypeRepository
    {
        private readonly PokemonDbContext _context;

        public SqlPokemonTypeDtoRepository(PokemonDbContext context)
        {
            _context = context;
        }

        public IEnumerable<PokemonTypeDto> GetAllTypes()
        {
            return _context.PokemonTypes
                .Select(t => new PokemonTypeDto
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .AsEnumerable();
        }
    }
}
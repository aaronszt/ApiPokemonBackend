using Pokemon.Domain.QueryModel.Repositories;
using Pokemon.Domain.QueryModel.Dtos;
using Pokemon.Persistence.Sql.Models;

namespace Pokemon.Persistence.Sql.Repositories.QueryModel
{
    public class SqlPokemonsDtoRepository : IPokemonsDtoRepository
    {
        private readonly PokemonDbContext _context;

        public SqlPokemonsDtoRepository(PokemonDbContext context) => _context = context;

        public IEnumerable<PokemonsDto> GetAllPokemons()
        {
            return _context.Pokemonss.Select(p => MapToDto(p)).AsEnumerable();
        }

        public PokemonsDto? GetPokemonById(int id)
        {
            return _context.Pokemonss
                .Where(p => p.Id == id)
                .Select(p => MapToDto(p))
                .FirstOrDefault();
        }

        public PokemonsDto? GetPokemonByName(string name)
        {
            return _context.Pokemonss
                .Where(p => p.Name.ToLower() == name.ToLower())
                .Select(p =>  MapToDto(p))
                .FirstOrDefault();

        }

        private static PokemonsDto MapToDto(Pokemones p) => new PokemonsDto
        {
            Id = p.Id,
            Name = p.Name,
            Image = p.Image,
            Attack = p.Attack,
            Defense = p.Defense,
            Hp = p.Hp,
            Height = p.Height,
            Weight = p.Weight,
            Speed = p.Speed,
            Custom = p.Custom,
            Types = p.PokemonTypes.Select(t => t.Name).ToList()
        };
    }
}
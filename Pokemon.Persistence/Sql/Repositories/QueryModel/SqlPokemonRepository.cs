using Microsoft.EntityFrameworkCore;
using Pokemon.Domain.Interfaces;
using Pokemon.Domain.Entities;

using Pokemon.Persistence.Sql.Models;

namespace Pokemon.Persistence.Sql.Repositories.QueryModel
{
    public class SqlPokemonRepository : IPokemonRepository
    {
        private readonly PokemonDbContext _context;

        public SqlPokemonRepository(PokemonDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CreatePokemon pokemon)
        {
            var newPokemon = new Pokemones
            {
                Name = pokemon.Name,
                Image = pokemon.Image,
                Hp = pokemon.Hp,
                Attack = pokemon.Attack,
                Defense = pokemon.Defense,
                Speed = pokemon.Speed,
                Height = pokemon.Height,
                Weight = pokemon.Weight,
                Custom = true,
                PokemonTypes = new List<PokemonType>()
            };

            var selectedTypes = await _context.PokemonTypes
                .Where(t => pokemon.TypeIds.Contains(t.Id))
                .ToListAsync();

            foreach (var type in selectedTypes)
            {
                newPokemon.PokemonTypes.Add(type);
            }

            await _context.Pokemonss.AddAsync(newPokemon);
        }

        public async Task DeleteAsync(int id)
        {
            var pokemon = await _context.Pokemonss.FindAsync(id);

            if (pokemon != null) _context.Remove(pokemon);
        }
    }
}
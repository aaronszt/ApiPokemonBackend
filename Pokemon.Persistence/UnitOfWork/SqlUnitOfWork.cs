using Pokemon.Persistence.Sql.Repositories.QueryModel;
using Pokemon.Domain.QueryModel.Repositories;
using Pokemon.Domain.Interfaces;
using Pokemon.Persistence.Sql;

namespace Pokemon.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly PokemonDbContext _context;

    public UnitOfWork(PokemonDbContext context)
    {
        _context = context;
    }

    IPokemonsDtoRepository IUnitOfWork.PokemonsDtoRepository => new SqlPokemonsDtoRepository(_context);
    IPokemonRepository IUnitOfWork.PokemonsRepository => new SqlPokemonRepository(_context);
    IPokemonTypeRepository IUnitOfWork.PokemonTypeRepository => new SqlPokemonTypeDtoRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
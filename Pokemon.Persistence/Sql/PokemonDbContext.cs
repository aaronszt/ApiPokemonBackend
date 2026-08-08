using Pokemon.Persistence.Sql.Models;
using Microsoft.EntityFrameworkCore;

namespace Pokemon.Persistence.Sql;

public partial class PokemonDbContext : DbContext
{
    public PokemonDbContext(){}

    public PokemonDbContext(DbContextOptions<PokemonDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Pokemones> Pokemonss { get; set; }
    public virtual DbSet<PokemonType> PokemonTypes { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pokemones>(entity =>
        {
           entity.HasKey(e => e.Id).HasName("PK__pokemon__3213E83F159B77B9");
           entity.ToTable("Pokemons");
           entity.Property(e => e.Id).HasColumnName("id");
           entity.Property(e => e.Name)
           .HasMaxLength(100)
           .HasColumnName("name")
           .IsRequired();
           entity.Property(e => e.Image).HasColumnName("image");
           entity.Property(e => e.Hp).HasColumnName("hp");
           entity.Property(e => e.Attack).HasColumnName("attack");
           entity.Property(e => e.Defense).HasColumnName("defense");
           entity.Property(e => e.Speed).HasColumnName("speed");
           entity.Property(e => e.Height).HasColumnName("height");
           entity.Property(e => e.Weight).HasColumnName("weight");
           entity.Property(e => e.Custom)
           .HasColumnName("custom")
           .HasDefaultValue(true);
           entity.Property(e => e.Name)
           .HasConversion(v => v.ToLower(), v => v);
        });

        modelBuilder.Entity<PokemonType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Types");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
            .HasColumnName("name")
            .IsRequired();            
            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<Pokemones>()
            .HasMany(p => p.PokemonTypes)
            .WithMany(t => t.Pokemons)
            .UsingEntity(j => j.ToTable("pokemon_types_relation"));

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

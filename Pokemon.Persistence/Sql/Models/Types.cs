using System.ComponentModel.DataAnnotations;

namespace Pokemon.Persistence.Sql.Models;

public class PokemonType
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    public virtual ICollection<Pokemones> Pokemons { get; set; } = new List<Pokemones>();
}
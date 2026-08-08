using System.ComponentModel.DataAnnotations;

namespace Pokemon.Persistence.Sql.Models;

public partial class Pokemones
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;
    public string? Image { get; set; }
    [Range(1, 200, ErrorMessage = "HP must be at least 1 and cannot exceed 200.")]
    public int Hp { get; set; }
    [Range(1, 200, ErrorMessage = "Attack must be at least 1 and cannot exceed 200.")]
    public int Attack { get; set; }
    [Range(1, 200, ErrorMessage = "Defense must be at least 1 and cannot exceed 200.")]
    public int Defense { get; set; }
    [Range(1, 200, ErrorMessage = "Speed must be at least 1 and cannot exceed 200.")]
    public int Speed { get; set; }
    [Range(1, 200, ErrorMessage = "Height must be at least 1 and cannot exceed 200.")]
    public int Height { get; set; }
    [Range(1, 1000, ErrorMessage = "Weight must be at least 1 and cannot exceed 1000.")]
    public int Weight { get; set; }
    public bool Custom { get; set; } = true;
    public virtual ICollection<PokemonType> PokemonTypes { get; set; } = new List<PokemonType>();
}
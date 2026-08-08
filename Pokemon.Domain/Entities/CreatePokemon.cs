namespace Pokemon.Domain.Entities
{
    public class CreatePokemon
    {
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
        public int Hp { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public List<int> TypeIds { get; set; } = new();
    }
}
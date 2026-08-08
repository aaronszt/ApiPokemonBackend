namespace Pokemon.Domain.QueryModel.Dtos
{
    public class PokemonsDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public int Hp { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public bool Custom { get; set; }
        public List<string> Types { get; set; } = new();
    }
}
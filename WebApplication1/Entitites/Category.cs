namespace WebApplication1.Entitites
{
    public class Category
    {
        public int Id { get; set; }
        public ICollection<MovieCategory> MovieCategories { get; set; } = new List<MovieCategory>();
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}

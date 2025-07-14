namespace WebApplication1.Entitites
{
    public class MovieCategory
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}

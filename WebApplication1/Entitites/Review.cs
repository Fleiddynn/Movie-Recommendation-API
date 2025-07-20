using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Entitites
{
    public class Review
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; } = null!;
        public string Note { get; set; } = string.Empty;
        public double Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

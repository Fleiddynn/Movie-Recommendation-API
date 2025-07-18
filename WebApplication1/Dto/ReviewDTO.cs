using System.ComponentModel.DataAnnotations;
using WebApplication1.Entitites;

namespace WebApplication1.Dto
{
    public class ReviewDTO
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public string Note { get; set; } = string.Empty;
        public double Rating { get; set; }
        public DateTime CreatedAt { get; set; }

    public ReviewDTO(Review review)
        {
            Id = review.Id;
            UserId = review.UserId;
            UserName = review.User?.UserName ?? string.Empty;
            MovieId = review.MovieId;
            MovieName = review.Movie?.Title ?? string.Empty;
            Note = review.Note;
            Rating = review.Rating;
            CreatedAt = review.CreatedAt;
        }
    }
}

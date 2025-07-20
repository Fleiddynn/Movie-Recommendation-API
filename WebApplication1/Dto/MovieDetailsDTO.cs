using System.ComponentModel.DataAnnotations;
using WebApplication1.Entitites;

namespace WebApplication1.Dto
{
    public class MovieDetailsDTO
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Duration { get; set; }
        public string Director { get; set; } = string.Empty;
        public List<Guid> Categories { get; set; } = [];
        public double IMDB { get; set; }
        public int Length { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public List<ReviewDTO> UserReviews { get; set; } = new List<ReviewDTO>();

        public MovieDetailsDTO(Movie movie, List<Review> reviews)
        {
            Id = movie.Id;
            Title = movie.Title;
            Description = movie.Description;
            Duration = movie.Duration;
            Director = movie.Director;
            Categories = movie.Categories;
            IMDB = movie.IMDB;
            Length = movie.Length;
            ReleaseDate = movie.ReleaseDate;
            UserReviews = reviews.Select(r => new ReviewDTO(r)).ToList();
        }
    }
}

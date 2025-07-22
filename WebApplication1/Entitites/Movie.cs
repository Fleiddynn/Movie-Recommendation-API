using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace WebApplication1.Entitites
{
    public class Movie
    {
        [Key]
        public Guid Id { get; set; }
        public ICollection<MovieCategory> MovieCategories { get; set; } = new List<MovieCategory>();
        public ICollection<Review> UserReviews { get; set; } = new List<Review>();
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Duration { get; set; }
        public string Director { get; set; } = string.Empty;
        public List<Guid> Categories { get; set; } = [];
        public double IMDB { get; set; }
        public int Length { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

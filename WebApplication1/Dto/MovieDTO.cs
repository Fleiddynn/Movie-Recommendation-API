using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace WebApplication1.Entitites
{
    public class MovieDTO
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

        public MovieDTO(Guid id, string title, string description, int duration, string director, List<Guid> categories, double imdb, int length, DateOnly releaseDate) 
        {
            Id = id;
            Title = title;
            Description = description;
            Duration = duration;
            Director = director;
            Categories = categories;
            IMDB = imdb;
            Length = length;
            ReleaseDate = releaseDate;
        }

        public MovieDTO(Movie m) 
        {
            Id = m.Id;
            Title = m.Title;
            Description = m.Description;
            Duration = m.Duration;
            Director = m.Director;
            Categories = m.Categories;
            IMDB = m.IMDB;
            Length = m.Length;
            ReleaseDate = m.ReleaseDate;
        }

    }
}

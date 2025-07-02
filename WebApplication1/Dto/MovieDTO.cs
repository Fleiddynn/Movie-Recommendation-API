using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace WebApplication1.Entitites
{
    public class MovieDTO
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Cast { get; set; } = new List<string>();
        public string Director { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = new List<string>();
        public double IMDB { get; set; }
        public int Length { get; set; }
        public DateOnly ReleaseDate { get; set; }

        public MovieDTO(int id, string title, string description, List<string> cast, string director, List<string> categories, double imdb, int length, DateOnly releaseDate) 
        {
            Id = id;
            Title = title;
            Description = description;
            Cast = cast;
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
            Cast = m.Cast;
            Director = m.Director;
            Categories = m.Categories;
            IMDB = m.IMDB;
            Length = m.Length;
            ReleaseDate = m.ReleaseDate;
        }

    }
}

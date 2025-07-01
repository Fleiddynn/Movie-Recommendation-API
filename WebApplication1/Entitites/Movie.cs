using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace WebApplication1.Entitites
{
    public class Movie
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


    }
}

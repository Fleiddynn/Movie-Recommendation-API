using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dto
{
    public class MovieAddDTO
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Duration { get; set; }
        public string Director { get; set; } = string.Empty;
        public List<Guid> Categories { get; set; } = [];
        public double IMDB { get; set; }
        public int Length { get; set; }
        public DateOnly? ReleaseDate { get; set; }
    }
}

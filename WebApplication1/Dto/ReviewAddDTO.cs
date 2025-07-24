using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dto
{
    public class ReviewAddDTO
    {
        public Guid UserId { get; set; }
        public Guid MovieId { get; set; }
        public string Note { get; set; } = string.Empty;
        public double Rating { get; set; }
    }
}

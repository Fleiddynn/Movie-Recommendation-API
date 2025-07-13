using WebApplication1.Entitites;

namespace WebApplication1.Dto
{
    public class CategoryDTO
    {
        public ICollection<MovieCategory> MovieCategories { get; set; } = new List<MovieCategory>();
        public string Name { get; set; } = string.Empty;

        public CategoryDTO(Category c)
        {
            MovieCategories = c.MovieCategories;
            Name = c.Name;
        }
    }
}

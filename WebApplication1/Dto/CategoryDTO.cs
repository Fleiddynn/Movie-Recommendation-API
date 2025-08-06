using WebApplication1.Entitites;

namespace WebApplication1.Dto
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public CategoryDTO(Category c)
        {
            Id = c.Id;
            Name = c.Name;
        }
    }
}

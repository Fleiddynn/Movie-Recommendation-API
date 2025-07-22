using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entitites;
using WebApplication1.Dto;
using WebApplication1.DbContexts;

namespace WebApplication1.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly AllDbContext _context;

        public CategoriesController(AllDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();
            if (categories == null || !categories.Any())
            {
                return NotFound("Hiç kategori bulunamadı.");
            }
            foreach (var category in categories)
            {
                categoryDTOs.Add(new CategoryDTO(category));
            }
            return Ok(categoryDTOs);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound($"Aradığınız kategori bulunamadı.");
            }
            CategoryDTO categoryDTO = new CategoryDTO(category);
            return Ok(categoryDTO);
        }
        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory([FromBody] CategoryAddDTO newCategoryDTO)
        {
            if (newCategoryDTO == null || string.IsNullOrWhiteSpace(newCategoryDTO.Name))
            {
                return BadRequest("Kategori adı boş olamaz.");
            }

            var category = new Category { Id = Guid.NewGuid(), Name = newCategoryDTO.Name, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryAddDTO updatedCategory)
        {
            var Category = await _context.Categories.FindAsync(id);
            if (Category == null)
            {
                return NotFound($"Aradığınız kategori bulunamadı.");
            }
            Category.Name = updatedCategory.Name;
            Category.UpdatedAt = DateTime.UtcNow;
            _context.Entry(Category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound($"Aradığınız kategori bulunamadı.");
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}

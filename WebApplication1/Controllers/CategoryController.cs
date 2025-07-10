using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entitites;
using WebApplication1.DbContexts.CategoryData;

namespace WebApplication1.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryDbContext _context;

        public CategoriesController(CategoryDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            if (categories == null || !categories.Any())
            {
                return NotFound("Hiç kategori bulunamadı.");
            }
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound($"Aradığınız kategori bulunamadı.");
            }
            return Ok(category);
        }
        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory([FromBody] Category newCategory)
        {
            if (newCategory == null || string.IsNullOrWhiteSpace(newCategory.Name))
            {
                return BadRequest("Kategori adı boş olamaz.");
            }
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategory), new { id = newCategory.Id }, newCategory);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category updatedCategory)
        {
            if (id != updatedCategory.Id)
            {
                return BadRequest("Kategori ID'leri değiştirilemez.");
            }
            var Category = await _context.Categories.FindAsync(id);
            if (Category == null)
            {
                return NotFound($"Aradığınız kategori bulunamadı.");
            }
            Category.Name = updatedCategory.Name;
            Category.UpdatedAt = DateTime.Now;
            _context.Entry(Category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound($"Aradığınız kategori bulunamadı.");
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}

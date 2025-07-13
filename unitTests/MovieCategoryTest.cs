using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Entitites;
using WebApplication1.DbContexts.CategoryData;
using Xunit;

namespace unitTests
{
    public class MovieCategoryTest
    {
        private CategoryDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<CategoryDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new CategoryDbContext(options);
        }

        private CategoriesController GetController(CategoryDbContext context)
        {
            return new CategoriesController(context);
        }

        [Fact]
        public async Task GetCategories()
        {
            var context = GetDbContext(nameof(GetCategories));
            context.Categories.Add(new Category { Id = 1, Name = "Test", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
            context.SaveChanges();
            var controller = GetController(context);

            var result = await controller.GetCategories();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var categories = Assert.IsAssignableFrom<IEnumerable<Category>>(okResult.Value);
            Assert.Single(categories);
        }

        [Fact]
        public async Task AddCategory()
        {
            var context = GetDbContext(nameof(AddCategory));
            var controller = GetController(context);
            var newCategory = new Category { Name = "Action", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };

            var result = await controller.AddCategory(newCategory);

            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            var category = Assert.IsType<Category>(created.Value);
            Assert.Equal("Action", category.Name);
        }
    }
}

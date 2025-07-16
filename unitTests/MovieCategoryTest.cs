using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Entitites;
using WebApplication1.DbContexts;
using WebApplication1.Dto;
using Xunit;

namespace unitTests
{
    public class MovieCategoryTest
    {
        private AllDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AllDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new AllDbContext(options);
        }

        private CategoriesController GetController(AllDbContext context)
        {
            return new CategoriesController(context);
        }

        [Fact]
        public async Task GetCategories()
        {
            var context = GetDbContext(nameof(GetCategories));
            context.Categories.Add(new Category { Id = new Guid(), Name = "Test", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
            context.SaveChanges();
            var controller = GetController(context);

            var result = await controller.GetCategories();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var categories = Assert.IsAssignableFrom<IEnumerable<CategoryDTO>>(okResult.Value);
            Assert.Single(categories);
        }

        [Fact]
        public async Task AddCategory()
        {
            var context = GetDbContext(nameof(AddCategory));
            var controller = GetController(context);
            var newCategory = new Category { Name = "Action", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            var categoryDto = new CategoryDTO(newCategory);

            var result = await controller.AddCategory(categoryDto);

            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            var category = Assert.IsType<Category>(created.Value);
            Assert.Equal("Action", category.Name);
        }
        [Fact]
        public async Task GetMovieByCategory()
        {
            var context = GetDbContext(nameof(GetMovieByCategory));
            var category = new Category { Id = Guid.NewGuid(), Name = "Action", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Categories.Add(category);
            context.Movies.Add(new Movie { Id = 1, Title = "Test Movie", Categories = new List<Guid> { category.Id } });
            context.SaveChanges();
            var controller = new MoviesController(context);
            var result = await controller.GetMoviesByCategory(category.Id);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var movies = Assert.IsAssignableFrom<IEnumerable<MovieDTO>>(okResult.Value);
            Assert.Single(movies);
        }
        [Fact]
        public async Task AddMovie()
        {
            var context = GetDbContext(nameof(AddMovie));
            var controller = new MoviesController(context);
            var newMovie = new Movie { Title = "The amazgin spiderman 2", Director = "MFH", IMDB = 9.9, Length = 145, ReleaseDate = new DateOnly(2006, 02, 02), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            var result = await controller.AddMovie(newMovie);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var movie = Assert.IsType<Movie>(createdResult.Value);
            Assert.Equal("The amazgin spiderman 2", movie.Title);
        }
        [Fact]
        public async Task UpdateMovie()
        {
            var context = GetDbContext(nameof(UpdateMovie));
            var movie = new Movie { Id = 1, Title = "The Amazing Spiderman 2", Director = "idk", IMDB = 7.4, Length = 243, ReleaseDate = new DateOnly(2011, 05, 04), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Movies.Add(movie);
            context.SaveChanges();
            var controller = new MoviesController(context);
            movie.Title = "The Amazing Spiderman 2";
            var result = await controller.UpdateMovie(movie.Id, movie);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedMovie = Assert.IsType<Movie>(okResult.Value);
            Assert.Equal(movie.Title, updatedMovie.Title);
        }
        [Fact]
        public async Task DeleteMovie()
        {
            var context = GetDbContext(nameof(DeleteMovie));
            var movie = new Movie { Id = 1, Title = "The Amazing Spiderman 2", Director = "idk", IMDB = 7.4, Length = 243, ReleaseDate = new DateOnly(2011, 05, 04), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Movies.Add(movie);
            context.SaveChanges();
            var controller = new MoviesController(context);
            var result = await controller.DeleteMovie(movie.Id);
            Assert.IsType<OkResult>(result);
            Assert.Null(context.Movies.Find(movie.Id));
        }
        [Fact]
        public async Task GetMovieById()
        {
            var context = GetDbContext(nameof(GetMovieById));
            var movie = new Movie { Id = 1, Title = "The Amazing Spiderman 2", Director = "idk", IMDB = 7.4, Length = 243, ReleaseDate = new DateOnly(2011, 05, 04), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Movies.Add(movie);
            context.SaveChanges();
            var controller = new MoviesController(context);
            var result = await controller.GetMovie(movie.Id);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedMovie = Assert.IsType<MovieDTO>(okResult.Value);
            Assert.Equal(movie.Title, returnedMovie.Title);
        }
    }
}

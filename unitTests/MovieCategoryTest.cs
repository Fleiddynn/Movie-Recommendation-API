﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Entitites;
using WebApplication1.DbContexts;
using WebApplication1.Dto;

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
            context.Movies.Add(new Movie { Id = Guid.NewGuid(), Title = "Test Movie", Categories = new List<Guid> { category.Id } });
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
            var movie = new Movie { Id = Guid.NewGuid(), Title = "The Amazing Spiderman 2", Director = "idk", IMDB = 7.4, Length = 243, ReleaseDate = new DateOnly(2011, 05, 04), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
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
            var movie = new Movie { Id = Guid.NewGuid(), Title = "The Amazing Spiderman 2", Director = "idk", IMDB = 7.4, Length = 243, ReleaseDate = new DateOnly(2011, 05, 04), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
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
            var movie = new Movie { Id = Guid.NewGuid(), Title = "The Amazing Spiderman 2", Director = "idk", IMDB = 7.4, Length = 243, ReleaseDate = new DateOnly(2011, 05, 04), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Movies.Add(movie);
            context.SaveChanges();
            var controller = new MoviesController(context);
            var result = await controller.GetMovie(movie.Id);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedMovie = Assert.IsType<MovieDTO>(okResult.Value);
            Assert.Equal(movie.Title, returnedMovie.Title);
        }
        [Fact]
        public async Task GetReviewByMovieId()
        {
            var context = GetDbContext(nameof(GetReviewByMovieId));
            var movie = new Movie { Id = Guid.NewGuid(), Title = "The Amazing Spiderman 2", Director = "idk", IMDB = 7.4, Length = 243, ReleaseDate = new DateOnly(2011, 05, 04) };
            context.Movies.Add(movie);
            var user = new User { Id = Guid.NewGuid(), first_name = "Muzo", last_name = "Heptas", email = "fleiddynn@gmail.com", password = "12334321" };
            context.Add(user);
            var review = new Review { Id = Guid.NewGuid(), MovieId = movie.Id, UserId = user.Id, Rating = 8, Note = "Çok iyi film bayıldım.", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Reviews.Add(review);
            await context.SaveChangesAsync();

            var controller = new ReviewController(context);
            var result = await controller.GetReviewsByMovieId(movie.Id);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var reviews = Assert.IsAssignableFrom<IEnumerable<ReviewDTO>>(okResult.Value);
            Assert.Single(reviews);
            var retrievedReview = reviews.First();
            Assert.Equal(review.Note, retrievedReview.Note);
            Assert.Equal(review.Rating, retrievedReview.Rating);
        }
        [Fact]
        public async Task AddReview()
        {
            var context = GetDbContext(nameof(GetReviewByMovieId));
            var movie = new Movie { Id = Guid.NewGuid(), Title = "The Amazing Spiderman 2", Director = "idk", IMDB = 7.4, Length = 243, ReleaseDate = new DateOnly(2011, 05, 04) };
            context.Movies.Add(movie);
            var user = new User { Id = Guid.NewGuid(), first_name = "Muzo", last_name = "Heptas", email = "fleiddynn@gmail.com", password = "12334321" };
            context.Add(user);
            var review = new Review { Id = Guid.NewGuid(), MovieId = movie.Id, UserId = user.Id, Rating = 8, Note = "Çok iyi film bayıldım.", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Reviews.Add(review);
            await context.SaveChangesAsync();

            var controller = new ReviewController(context);

            var newReview = new Review { MovieId = movie.Id, UserId = user.Id, Rating = 8, Note = "Çok iyi film bayıldım." };
            var result = await controller.AddReview(newReview);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var reviewDto = Assert.IsType<ReviewDTO>(createdResult.Value);
            Console.WriteLine(reviewDto);
            Assert.Equal("Çok iyi film bayıldım.", reviewDto.Note);
        }
        [Fact]
        public async Task UpdateReview()
        {
            var context = GetDbContext(nameof(GetReviewByMovieId));
            var movie = new Movie { Id = Guid.NewGuid(), Title = "The Amazing Spiderman 2", Director = "idk", IMDB = 7.4, Length = 243, ReleaseDate = new DateOnly(2011, 05, 04) };
            context.Movies.Add(movie);
            var user = new User { Id = Guid.NewGuid(), first_name = "Muzo", last_name = "Heptas", email = "fleiddynn@gmail.com", password = "12334321" };
            context.Add(user);
            var review = new Review { Id = Guid.NewGuid(), MovieId = movie.Id, UserId = user.Id, Rating = 8, Note = "Çok iyi film bayıldım.", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Reviews.Add(review);
            await context.SaveChangesAsync();

            var controller = new ReviewController(context);

            var updatedReviewData = new Review { Id = review.Id, MovieId = movie.Id, UserId = user.Id, Rating = 2, Note = "Berbattı." };

            var result = await controller.UpdateReview(updatedReviewData.Id, updatedReviewData);
            Assert.IsType<OkResult>(result);

            var updatedReviewInDb = await context.Reviews.FindAsync(review.Id);
            Assert.NotNull(updatedReviewInDb);
            Assert.Equal("Berbattı.", updatedReviewInDb.Note);
            Assert.Equal(2, updatedReviewInDb.Rating);

            var movieInDb = await context.Movies.FindAsync(movie.Id);
            Assert.NotNull(movieInDb);
            Assert.Equal(2.0, movieInDb.IMDB);
            // Bir tane daha review atanıp filmin imdbsi ortalamaya göre updatelenecekmi diye kontorl edilebilir
        }
        [Fact]
        public async Task DeleteReview()
        {
            var context = GetDbContext(nameof(DeleteReview));
            var user = new User { Id = Guid.NewGuid(), first_name = "Muzo", last_name = "Heptas", email = "fleiddynn@gmail.com", password = "12334321", UserName = "SDenem" };
            context.Users.Add(user);
            var movie = new Movie { Id = Guid.NewGuid(), Title = "The Amazing Spiderman 2", Director = "idk", IMDB = 7.4, Length = 243, ReleaseDate = new DateOnly(2011, 05, 04) };
            context.Movies.Add(movie);
            var review = new Review { Id = Guid.NewGuid(), MovieId = movie.Id, UserId = user.Id, Rating = 8, Note = "Çok iyi film bayıldım.", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Reviews.Add(review);
            await context.SaveChangesAsync();

            var controller = new ReviewController(context);
            var result = await controller.DeleteReview(review.Id);
            Assert.IsType<NoContentResult>(result);
            Assert.Null(await context.Reviews.FindAsync(review.Id));

            var movieInDb = await context.Movies.FindAsync(movie.Id);
            Assert.NotNull(movieInDb);
            Assert.Equal(0.0, movieInDb.IMDB);
        }
        [Fact]
        public async Task GetReviewsByMovieId()
        {
            var context = GetDbContext(nameof(GetReviewByMovieId));
            var movie = new Movie { Id = Guid.NewGuid(), Title = "The Amazing Spiderman 2", Director = "idk", IMDB = 7.4, Length = 243, ReleaseDate = new DateOnly(2011, 05, 04) };
            context.Movies.Add(movie);
            var user = new User { Id = Guid.NewGuid(), first_name = "Muzo", last_name = "Heptas", email = "fleiddynn@gmail.com", password = "12334321" };
            context.Add(user);
            var review = new Review { Id = Guid.NewGuid(), MovieId = movie.Id, UserId = user.Id, Rating = 8, Note = "Çok iyi film bayıldım.", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Reviews.Add(review);
            await context.SaveChangesAsync();

            var controller = new ReviewController(context);
            var result = await controller.GetReviewsByMovieId(movie.Id);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var reviews = Assert.IsAssignableFrom<IEnumerable<ReviewDTO>>(okResult.Value);
            Assert.Single(reviews);
        }
        [Fact]
        public async Task GetWatchlist()
        {
            var context = GetDbContext(nameof(GetWatchlist));
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, first_name = "Muzo", last_name = "Heptas", email = "fleiddynn@gmail.com", password = "12334321" };
            context.Add(user);
            var movieId = Guid.NewGuid();
            var movie = new Movie { Id = movieId, Title = "The Amazing Spiderman 2", Director = "idk", IMDB = 7.4, Length = 243, ReleaseDate = new DateOnly(2011, 05, 04), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Movies.Add(movie);
            user.watchedMovies.Add(movie.Id);
            context.SaveChanges();
            var controller = new UserController(context);
            var result = await controller.GetWatchlist(userId);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var watchlist = Assert.IsAssignableFrom<IEnumerable<MovieDTO>>(okResult.Value);
            Assert.Single(watchlist);
        }
        [Fact]
        public async Task AddToWatchlist()
        {
            var context = GetDbContext(nameof(AddToWatchlist));
            var moiveId = Guid.NewGuid();
            var movie = new Movie { Id = moiveId, Title = "The Amazing Spiderman 2", Director = "idk", IMDB = 7.4, Length = 243, ReleaseDate = new DateOnly(2011, 05, 04), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Movies.Add(movie);
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, first_name = "Muzo", last_name = "Heptas", email = "fleiddynn@gmail.com", password = "12334321" };
            context.Add(user);
            context.SaveChanges();

            var controller = new UserController(context);
            var result = await controller.AddToWatchlist(userId, moiveId);
            Assert.IsType<OkResult>(result);
        }
        [Fact]
        private async Task Deneme()
        {
            Assert.Equal(1, 1);
        }
    }
}

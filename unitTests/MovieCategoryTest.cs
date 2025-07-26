using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Entitites;
using WebApplication1.DbContexts;
using WebApplication1.Dto;
using Microsoft.AspNetCore.JsonPatch;

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
            context.Categories.Add(new Category { Id = Guid.NewGuid(), Name = "Aksiyon", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
            context.SaveChanges();
            var controller = GetController(context);

            var result = await controller.GetCategories();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var categories = Assert.IsAssignableFrom<IEnumerable<CategoryDTO>>(okResult.Value);
            Assert.Single(categories);
        }
        [Fact]
        public async Task GetCategoryById()
        {
            var context = GetDbContext(nameof(GetCategoryById));
            var category = new Category { Id = Guid.NewGuid(), Name = "Aksiyon", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Categories.Add(category);
            context.SaveChanges();

            var controller = GetController(context);
            var result = await controller.GetCategory(category.Id);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCategory = Assert.IsType<CategoryDTO>(okResult.Value);
            Assert.Equal(category.Name, returnedCategory.Name);
        }

        [Fact]
        public async Task AddCategory()
        {
            var context = GetDbContext(nameof(AddCategory));
            var controller = GetController(context);
            var newCategoryDto = new CategoryAddDTO { Name = "Action"};

            var result = await controller.AddCategory(newCategoryDto);

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
        public async Task UpdateCategory()
        {
            var context = GetDbContext(nameof(UpdateCategory));
            var originalCategory = new Category { Id = Guid.NewGuid(), Name = "Action", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Categories.Add(originalCategory);
            context.SaveChanges();

            var controller = GetController(context);
            var updatedName = "Adventure";
            var categoryDto = new CategoryAddDTO { Name = updatedName };
            var result = await controller.UpdateCategory(originalCategory.Id, categoryDto);
            Assert.IsType<OkResult>(result);

            var updatedCategoryInDb = await context.Categories.FindAsync(originalCategory.Id);
            Assert.NotNull(updatedCategoryInDb);
            Assert.Equal(updatedName, updatedCategoryInDb.Name);
        }
        [Fact]
        public async Task DeleteCategory()
        {
            var context = GetDbContext(nameof(DeleteCategory));
            var category = new Category { Id = Guid.NewGuid(), Name = "Action", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Categories.Add(category);
            context.SaveChanges();
            var controller = GetController(context);
            var result = await controller.DeleteCategory(category.Id);
            Assert.IsType<OkResult>(result);
            var deletedCategory = await context.Categories.FindAsync(category.Id);
            Assert.Null(deletedCategory);
        }
        [Fact]
        public async Task AddMovie()
        {
            var context = GetDbContext(nameof(AddMovie));
            var controller = new MoviesController(context);
            var newMovieDto = new MovieAddDTO { Title = "The amazgin spiderman 2", Director = "MFH", IMDB = 9.9, Length = 145, Duration = 23, Categories = new List<Guid>(), ReleaseDate = new DateOnly(2006, 02, 02)};
            var result = await controller.AddMovie(newMovieDto);
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
            var movieDto = new MovieAddDTO { Title = "The Amazing Spiderman 2", Director = "idk", IMDB = 7.4, Length = 243, ReleaseDate = new DateOnly(2011, 05, 04) };
            movie.Title = "The Amazing Spiderman 2";
            var result = await controller.UpdateMovie(movie.Id, movieDto);
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
        public async Task GetMovies()
        {
            var context = GetDbContext(nameof(GetMovies));
            var movie1 = new Movie { Id = Guid.NewGuid(), Title = "The Amazing Spiderman 2", Director = "idk", IMDB = 7.4, Length = 243, ReleaseDate = new DateOnly(2011, 05, 04), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            var movie2 = new Movie { Id = Guid.NewGuid(), Title = "The Amazing Spiderman", Director = "idk", IMDB = 8.4, Length = 243, ReleaseDate = new DateOnly(2009, 05, 04), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Movies.Add(movie1);
            context.Movies.Add(movie2);
            context.SaveChanges();
            var controller = new MoviesController(context);
            var result = await controller.GetMovies();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var movies = Assert.IsAssignableFrom<IEnumerable<MovieDTO>>(okResult.Value);
            Assert.Equal(2, movies.Count());
        }
        [Fact]
        public async Task PartiallyUpdateMovie()
        {
            var context = GetDbContext(nameof(PartiallyUpdateMovie));
            var movie = new Movie { Id = Guid.NewGuid(), Title = "The Amazing Spiderman 2", Director = "idk", IMDB = 7.4, Length = 243, ReleaseDate = new DateOnly(2011, 05, 04), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Movies.Add(movie);
            context.SaveChanges();
            var controller = new MoviesController(context);
            var patchDoc = new JsonPatchDocument<Movie>();
            patchDoc.Replace(m => m.Length, 255 );
            var result = await controller.PartiallyUpdateMovie(movie.Id, patchDoc);
            Assert.IsType<OkResult>(result);
            var updatedMovie = await context.Movies.FindAsync(movie.Id);
            Assert.NotNull(updatedMovie);
            Assert.Equal(255, updatedMovie.Length);
        }
        [Fact]
        public async Task AddCategoryToMovie()
        {
            var context = GetDbContext(nameof(AddCategoryToMovie));
            var movie = new Movie { Id = Guid.NewGuid(), Title = "The Amazing Spiderman 2", Director = "idk", IMDB = 7.4, Length = 243, ReleaseDate = new DateOnly(2011, 05, 04), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Movies.Add(movie);
            var category = new Category { Id = Guid.NewGuid(), Name = "Action", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Categories.Add(category);
            context.SaveChanges();
            var controller = new MoviesController(context);
            var result = await controller.AddCategoryToMovie(movie.Id, category.Id);
            var updatedMovie = await context.Movies.FindAsync(movie.Id);
            Assert.NotNull(updatedMovie);
            Assert.Contains(category.Id, updatedMovie.Categories);
        }
        [Fact]
        public async Task GetMoviesByCategoryWithPagination()
        {
            var context = GetDbContext(nameof(GetMoviesByCategoryWithPagination));
            var category = new Category { Id = Guid.NewGuid(), Name = "Action", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Categories.Add(category);
            var movie1 = new Movie { Id = Guid.NewGuid(), Title = "The Amazing Spiderman 2", Director = "idk", IMDB = 7.4, Length = 243, ReleaseDate = new DateOnly(2011, 05, 04), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            var movie2 = new Movie { Id = Guid.NewGuid(), Title = "The Amazing Spiderman", Director = "idk", IMDB = 8.4, Length = 243, ReleaseDate = new DateOnly(2009, 05, 04), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Movies.Add(movie1);
            context.Movies.Add(movie2);
            movie1.Categories.Add(category.Id);
            movie2.Categories.Add(category.Id);
            await context.SaveChangesAsync();

            var controller = new MoviesController(context);
            var result = await controller.GetMoviesByCategoryWithPagination(category.Id, 1, 10);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var movies = Assert.IsAssignableFrom<IEnumerable<MovieDTO>>(okResult.Value);
            Assert.Equal(2, movies.Count());
        }
        [Fact]
        public async Task GetMovieWithAllDetails()
        {
            var context = GetDbContext(nameof(GetMovieWithAllDetails));
            var movie = new Movie { Id = Guid.NewGuid(), Title = "The Amazing Spiderman 2", Director = "idk", IMDB = 7.4, Length = 243, ReleaseDate = new DateOnly(2011, 05, 04), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Movies.Add(movie);
            var user = new User { Id = Guid.NewGuid(), first_name = "Muzo", last_name = "Heptas", email = "fleiddynn@gmail.com", password = "12334321" };
            context.Add(user);
            var review = new Review { Id = Guid.NewGuid(), MovieId = movie.Id, UserId = user.Id, Rating = 8, Note = "Çok iyi film bayıldım.", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Reviews.Add(review);
            context.SaveChanges();
            var controller = new MoviesController(context);
            var result = await controller.GetMovieWithAllDetails(movie.Id);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedMovie = Assert.IsType<MovieDetailsDTO>(okResult.Value);
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
        public async Task GetReviewByUserId()
        {
            var context = GetDbContext(nameof(GetReviewByUserId));
            var movie = new Movie { Id = Guid.NewGuid(), Title = "The Amazing Spiderman 2", Director = "idk", IMDB = 7.4, Length = 243, ReleaseDate = new DateOnly(2011, 05, 04) };
            context.Movies.Add(movie);
            var user = new User { Id = Guid.NewGuid(), first_name = "Muzo", last_name = "Heptas", email = "fleiddynn@gmail.com", password = "12334321" };
            context.Add(user);
            var review = new Review { Id = Guid.NewGuid(), MovieId = movie.Id, UserId = user.Id, Rating = 8, Note = "Çok iyi film bayıldım.", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Reviews.Add(review);
            await context.SaveChangesAsync();

            var controller = new ReviewController(context);
            var result = await controller.GetReviewsByUserId(user.Id);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var reviews = Assert.IsAssignableFrom<IEnumerable<ReviewDTO>>(okResult.Value);
            Assert.Single(reviews);
            var retrievedReview = reviews.First();
            Assert.Equal(review.Note, retrievedReview.Note);
            Assert.Equal(review.Rating, retrievedReview.Rating);
            Assert.Equal(user.Id, retrievedReview.UserId);
        }
        [Fact] 
        public async Task GetReviewByMovieAndUser()
        {
            var context = GetDbContext(nameof(GetReviewByMovieAndUser));
            var movie = new Movie { Id = Guid.NewGuid(), Title = "The Amazing Spiderman 2", Director = "idk", IMDB = 7.4, Length = 243, ReleaseDate = new DateOnly(2011, 05, 04) };
            context.Movies.Add(movie);
            var user = new User { Id = Guid.NewGuid(), first_name = "Muzo", last_name = "Heptas", email = "fleiddynn@gmail.com", password = "12334321" };
            context.Add(user);
            var review = new Review { Id = Guid.NewGuid(), MovieId = movie.Id, UserId = user.Id, Rating = 8, Note = "Çok iyi film bayıldım.", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Reviews.Add(review);
            await context.SaveChangesAsync();

            var controller = new ReviewController(context);
            var result = await controller.GetReviewByMovieAndUser(movie.Id, user.Id);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var reviewDto = Assert.IsType<ReviewDTO>(okResult.Value);
            Assert.Equal(review.Note, reviewDto.Note);
            Assert.Equal(review.Rating, reviewDto.Rating);
            Assert.Equal(user.Id, reviewDto.UserId);
            Assert.Equal(movie.Id, reviewDto.MovieId);
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

            var newReview = new ReviewAddDTO { MovieId = movie.Id, UserId = user.Id, Rating = 8, Note = "Çok iyi film bayıldım." };
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

            var updatedReviewData = new ReviewAddDTO { MovieId = movie.Id, UserId = user.Id, Rating = 2, Note = "Berbattı." };

            var result = await controller.UpdateReview(review.Id, updatedReviewData);
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
        public async Task GetAllReviews()
        {
            var context = GetDbContext(nameof(GetAllReviews));
            var user = new User { Id = Guid.NewGuid(), first_name = "Muzo", last_name = "Heptas", email = "fleiddynn@gmail.com", password = "12334321", UserName = "SDenem" };
            context.Users.Add(user);
            var movie = new Movie { Id = Guid.NewGuid(), Title = "The Amazing Spiderman 2", Director = "idk", IMDB = 7.4, Length = 243, ReleaseDate = new DateOnly(2011, 05, 04) };
            context.Movies.Add(movie);
            var review = new Review { Id = Guid.NewGuid(), MovieId = movie.Id, UserId = user.Id, Rating = 8, Note = "Çok iyi film bayıldım.", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Reviews.Add(review);
            user.Id = Guid.NewGuid();
            context.Users.Add(user);
            movie.Id = Guid.NewGuid();
            context.Movies.Add(movie);
            context.Reviews.Add(new Review { Id = Guid.NewGuid(), MovieId = movie.Id, UserId = user.Id, Rating = 2, Note = "Berbat fil", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
            await context.SaveChangesAsync();

            var controller = new ReviewController(context);
            var result = await controller.GetAllReviews();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var reviews = Assert.IsAssignableFrom<IEnumerable<ReviewDTO>>(okResult.Value);
            Assert.Equal(2, reviews.Count());
            Assert.Contains(reviews, r => r.Note == "Çok iyi film bayıldım.");
            Assert.Contains(reviews, r => r.Note == "Berbat fil");
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
        public async Task RemoveFromWatchlist()
        {
            var context = GetDbContext(nameof(RemoveFromWatchlist));
            var moiveId = Guid.NewGuid();
            var movie = new Movie { Id = moiveId, Title = "The Amazing Spiderman 2", Director = "idk", IMDB = 7.4, Length = 243, ReleaseDate = new DateOnly(2011, 05, 04), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            context.Movies.Add(movie);
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, first_name = "Muzo", last_name = "Heptas", email = "fleiddynn@gmail.com", password = "12334321" };
            context.Add(user);
            user.watchedMovies.Add(moiveId);
            context.SaveChanges();

            var controller = new UserController(context);
            var result = await controller.RemoveFromWatchlist(userId, moiveId);
            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public async Task GetUsers()
        {
            var context = GetDbContext(nameof(GetUsers));
            var user = new User { Id = Guid.NewGuid(), first_name = "Muzo", last_name = "Heptas", email = "fleiddynn@gmail.com", password = "12334321", UserName = "SDenem" };
            context.Add(user);
            context.SaveChanges();

            var controller = new UserController(context);
            var result = await controller.GetUsers();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var users = Assert.IsAssignableFrom<IEnumerable<UserDTO>>(okResult.Value);
            Assert.Single(users);
        }
        [Fact] 
        public async Task GetUser()
        {
            var context = GetDbContext(nameof(GetUser));
            var user = new User { Id = Guid.NewGuid(), first_name = "Muzo", last_name = "Heptas", email = "fleiddynn@gmail.com", password = "12334321", UserName = "SDenem" };
            context.Add(user);
            context.SaveChanges();

            var controller = new UserController(context);
            var result = await controller.GetUser(user.Id);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsType<UserDTO>(okResult.Value);
            Assert.Equal(user.first_name, returnedUser.first_name);
        }
        [Fact]
        public async Task LoginAndReg()
        {
            var context = GetDbContext(nameof(LoginAndReg));
            var user = new UserChDTO { first_name = "Muzo", last_name = "Heptas", email = "fleiddynn@gmail.com", password = "12334321"};
            var controller = new UserController(context);
            var registerResult = await controller.Register(user);
            Assert.IsType<CreatedAtActionResult>(registerResult);
            context.SaveChanges();
            //Üst kısım çalışıyor.

            var loginDto = new UserLoginDTO { email = user.email, password = user.password };
            var result = await controller.Login(loginDto);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var loggedInUser = Assert.IsType<UserDTO>(okResult.Value);
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task UpdateUser()
        {
            var context = GetDbContext(nameof(UpdateUser));
            var user = new User { Id = Guid.NewGuid(), first_name = "Muzo", last_name = "Heptas", email = "fleiddynn@gmail.com", password = "12334321", UserName = "SDenem" };
            context.Add(user);
            context.SaveChanges();

            var updatedUserDto = new UserChDTO { first_name = "Furkn", last_name = "Heptas", email = "fleiddynn@gmail.com", password = "12334321" };

            var controller = new UserController(context);
            var result = await controller.UpdateUser(user.Id, updatedUserDto);

            Assert.IsType<OkResult>(result);

            var userInDb = await context.Users.FindAsync(user.Id);

            Assert.NotNull(userInDb);
            Assert.Equal(updatedUserDto.first_name, userInDb.first_name);
        }
        [Fact]
        public async Task DeleteUser()
        {
            var context = GetDbContext(nameof(DeleteUser));
            var user = new User { Id = Guid.NewGuid(), first_name = "Muzo", last_name = "Heptas", email = "fleiddynn@gmail.com", password = "12334321", UserName = "SDenem" };
            context.Add(user);
            context.SaveChanges();

            var controller = new UserController(context);
            var result = await controller.DeleteUser(user.Id);
            Assert.IsType<OkResult>(result);
        }


        [Fact]
        private async Task Deneme()
        {
            Assert.Equal(1, 1);
        }
    }
}

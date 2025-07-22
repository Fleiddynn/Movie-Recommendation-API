using WebApplication1.Entitites;

namespace WebApplication1.DbContexts.MovieRecData
{
    public interface IMovieRepository
    {
        public Task<List<Movie>> GetMoviesAsync();
        public Task<Movie?> GetMovieByIdAsync(Guid id);
        public Task<List<Movie>> GetMoviesByCategoryAsync(Guid categoryId);
        public Task<Movie> Create(Movie movie);
        public Task<Movie?> Update(Movie movie);
        public Task<Movie?> Delete(Guid id);
        public Task<List<Movie>> GetMoviesByCategoryAsync(Guid categoryId, int pageNumber, int pageSize);
        public Task<List<Movie>> GetMoviesAsync(string? sortBy, string? sortOrder);
        public Task<List<Review>> GetReviewsByMovieAsync(Guid id);
    }
}

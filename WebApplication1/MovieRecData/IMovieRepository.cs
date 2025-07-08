using WebApplication1.Entitites;

namespace WebApplication1.MovieData
{
    public interface IMovieRepository
    {
        public Task<List<Movie>> GetMoviesAsync();
        public Task<Movie?> GetMovieByIdAsync(int id);
        public Task<Movie> Create(Movie movie);
        public Task<Movie?> Update(Movie movie);
        public Task<Movie?> Delete(int id);
    }
}

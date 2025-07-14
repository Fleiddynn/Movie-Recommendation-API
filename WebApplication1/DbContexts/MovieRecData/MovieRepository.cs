using Microsoft.EntityFrameworkCore;
using WebApplication1.Entitites;

namespace WebApplication1.DbContexts.MovieRecData
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AllDbContext _context;
        public MovieRepository(AllDbContext context)
        {
            _context = context;
        }
        public Task<List<Movie>> GetMoviesAsync()
        {
            var movies = _context.Movies.ToList();
            return Task.FromResult(movies);
        }
        public Task<Movie?> GetMovieByIdAsync(int id)
        {
            var movie = _context.Movies.Find(id);
            return Task.FromResult(movie);
        }
        public async Task<List<Movie>> GetMoviesByCategoryAsync(Guid categoryId)
        {
            return await _context.Movies
                .Where(m => m.Categories.Contains(categoryId))
                .ToListAsync();
        }
        public Task<Movie> Create(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
            return Task.FromResult(movie);
        }
        public Task<Movie?> Update(Movie movie)
        {
            _context.Movies.Update(movie);
            _context.SaveChanges();
            return Task.FromResult(movie);
        }
        public Task<Movie?> Delete(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }
            return Task.FromResult(movie);
        }
        public async Task<List<Movie>> GetMoviesByCategoryAsync(Guid categoryId, int pageNumber, int pageSize)
        {
            return await _context.Movies
                .Where(m => m.Categories.Contains(categoryId))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}

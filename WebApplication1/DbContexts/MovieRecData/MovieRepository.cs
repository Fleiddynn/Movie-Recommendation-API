﻿using Microsoft.EntityFrameworkCore;
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
        public Task<Movie?> GetMovieByIdAsync(Guid id)
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
        public Task<Movie?> Delete(Guid id)
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
        public async Task<List<Movie>> GetMoviesAsync(string? sortBy, string? sortOrder)
        {
            IQueryable<Movie> movies = _context.Movies;

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy.ToLowerInvariant())
                {
                    case "title":
                        movies = (sortOrder?.ToLowerInvariant() == "desc") ?
                            movies.OrderByDescending(m => m.Title) :
                            movies.OrderBy(m => m.Title);
                        break;
                    case "imdb":
                        movies = (sortOrder?.ToLowerInvariant() == "desc") ?
                            movies.OrderByDescending(m => m.IMDB) :
                            movies.OrderBy(m => m.IMDB);
                        break;
                    case "releasedate":
                        movies = (sortOrder?.ToLowerInvariant() == "desc") ?
                            movies.OrderByDescending(m => m.ReleaseDate) :
                            movies.OrderBy(m => m.ReleaseDate);
                        break;
                    case "duration":
                        movies = (sortOrder?.ToLowerInvariant() == "desc") ?
                            movies.OrderByDescending(m => m.Duration) :
                            movies.OrderBy(m => m.Duration);
                        break;
                    case "createdat":
                        movies = (sortOrder?.ToLowerInvariant() == "desc") ?
                            movies.OrderByDescending(m => m.CreatedAt) :
                            movies.OrderBy(m => m.CreatedAt);
                        break;
                    default:
                        movies = movies.OrderBy(m => m.Id);
                        break;
                }
            }
            else
            {
                movies = movies.OrderBy(m => m.Id);
            }

            return await movies.ToListAsync();
        }
        public async Task<List<Review>> GetReviewsByMovieAsync(Guid id)
        {
            return await _context.Reviews
                .Where(r => r.MovieId == id)
                .ToListAsync();
        }
    }
    
}

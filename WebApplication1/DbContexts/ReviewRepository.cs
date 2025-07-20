using WebApplication1.Entitites;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.DbContexts
{
    public class ReviewRepository : IReviewRepository
    {
        public readonly AllDbContext _context;
        public ReviewRepository(AllDbContext context)
        {
            _context = context;
        }
        public async Task<List<Review>> GetReviewsAsync()
        {
            return await _context.Reviews.ToListAsync();
        }
        public async Task<Review?> GetReviewByIdAsync(Guid id)
        {
            return await _context.Reviews.FindAsync(id);
        }
        public async Task<List<Review>> GetReviewByMovieIdAsync(Guid movieId)
        {
            return await _context.Reviews
                .Where(r => r.MovieId == movieId)
                .ToListAsync();
        }
        public async Task<List<Review>> GetReviewsByUserIdAsync(Guid userId)
        {
            return await _context.Reviews
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
        public async Task<List<Review>> GetReviewsByUserAndMovieIdAsync(Guid userId, Guid movieId)
        {
            return await _context.Reviews
                .Where(r => r.UserId == userId && r.MovieId == movieId)
                .ToListAsync();
        }
        public async Task<Review> Create(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }
        public async Task<Review> Update(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
            return review;
        }
        public async Task<Review?> Delete(Guid id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
            return review;
        }
    }
}

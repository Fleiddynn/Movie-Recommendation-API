using WebApplication1.Entitites;

namespace WebApplication1.DbContexts
{
    public interface IReviewRepository
    {
        public Task<List<Review>> GetReviewsAsync();
        public Task<Review?> GetReviewByIdAsync(int id);
        public Task<List<Review>> GetReviewByMovieIdAsync(int movieId);
        public Task<List<Review>> GetReviewsByUserIdAsync(string userId);
        public Task<List<Review>> GetReviewsByUserAndMovieIdAsync(string userId, int movieId);
        public Task<Review> Create(Review review);
        public Task<Review> Update(Review review);
        public Task<Review?> Delete(int id);
    }
}

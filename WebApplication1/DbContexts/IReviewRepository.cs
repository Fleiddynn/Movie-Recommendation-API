using WebApplication1.Entitites;

namespace WebApplication1.DbContexts
{
    public interface IReviewRepository
    {
        public Task<List<Review>> GetReviewsAsync();
        public Task<Review?> GetReviewByIdAsync(Guid id);
        public Task<List<Review>> GetReviewByMovieIdAsync(Guid movieId);
        public Task<List<Review>> GetReviewsByUserIdAsync(Guid userId);
        public Task<List<Review>> GetReviewsByUserAndMovieIdAsync(Guid userId, Guid movieId);
        public Task<Review> Create(Review review);
        public Task<Review> Update(Review review);
        public Task<Review?> Delete(Guid id);
    }
}

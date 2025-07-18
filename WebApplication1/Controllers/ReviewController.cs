using Microsoft.AspNetCore.Mvc;
using WebApplication1.Entitites;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DbContexts;

namespace WebApplication1.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly AllDbContext _context;
        public ReviewController(AllDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReview>>> GetAllReviews()
        {
            var reviews = await _context.UserReviews.ToListAsync();
            if (reviews == null || !reviews.Any())
            {
                return NotFound("Hiç kullanıcı incelemesi bulunamadı.");
            }
            return Ok(reviews);

        }

        [HttpGet("{movieId}")]
        public async Task<ActionResult<IEnumerable<UserReview>>> GetReviewsByMovieId(int movieId)
        {
            var reviews = await _context.UserReviews
                .Where(r => r.MovieId == movieId)
                .ToListAsync();
            if (reviews == null || !reviews.Any())
            {
                return NotFound("Bu film için hiç kullanıcı yorumu bulunamadı.");
            }
            return Ok(reviews);
        }
        [HttpPost]
        public async Task<ActionResult<UserReview>> AddReview([FromBody] UserReview newReview)
        {
            if (newReview == null)
            {
                return BadRequest("İnceleme bilgileri boş olamaz.");
            }

            var existingReview = await _context.UserReviews
                .FirstOrDefaultAsync(r => r.MovieId == newReview.MovieId && r.UserId == newReview.UserId);

            if (existingReview != null)
            {
                return BadRequest("Bu kullanıcı zaten bu filmi incelemiş.");
            }
            if (newReview.Rating < 0 || newReview.Rating > 10)
            {
                return BadRequest("Verilen puan 0 ila 10 arasında olmalıdır.");
            }

            newReview.CreatedAt = DateTime.Now;
            newReview.UpdatedAt = DateTime.Now;

            _context.UserReviews.Add(newReview);
            await _context.SaveChangesAsync();

            var movie = await _context.Movies.FindAsync(newReview.MovieId);
            var reviewsForMovie = await _context.UserReviews
                                       .Where(r => r.MovieId == newReview.MovieId)
                                       .ToListAsync();
            if (reviewsForMovie.Any())
            {
                double newAverageRating = reviewsForMovie.Average(r => r.Rating);
                movie.IMDB = Math.Round(newAverageRating, 1);
            }
            else
            {
                movie.IMDB = newReview.Rating;
            }
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReviewsByMovieId), new { movieId = newReview.MovieId }, newReview);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] UserReview updatedReview)
        {
            if (id != updatedReview.Id)
            {
                return BadRequest("İnceleme ID'leri değiştirilemez.");
            }
            var Review = await _context.UserReviews.FindAsync(id);
            if (Review == null)
            {
                return NotFound("Güncellenecek inceleme bulunamadı.");
            }
            Review.Note = updatedReview.Note;
            if (updatedReview.Rating < 0 || updatedReview.Rating > 10)
            {
                return BadRequest("Verilen puan 0 ila 10 arasında olmalıdır.");
            }
            Review.Rating = updatedReview.Rating;
            Review.UpdatedAt = DateTime.Now;
            _context.UserReviews.Update(Review);
            await _context.SaveChangesAsync();
            var movie = await _context.Movies.FindAsync(Review.MovieId);
            var reviewsForMovie = await _context.UserReviews
                                       .Where(r => r.MovieId == Review.MovieId)
                                       .ToListAsync();
            if (reviewsForMovie.Any())
            {
                double newAverageRating = reviewsForMovie.Average(r => r.Rating);
                movie.IMDB = Math.Round(newAverageRating, 1);
            }
            else
            {
                movie.IMDB = Review.Rating;
            }
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.UserReviews.FindAsync(id);
            if (review == null)
            {
                return NotFound("Silinecek inceleme bulunamadı.");
            }
            var movieId = review.MovieId;
            _context.UserReviews.Remove(review);
            await _context.SaveChangesAsync();

            var movie = await _context.Movies.FindAsync(movieId);
            if (movie != null)
            {
                var reviewsForMovie = await _context.UserReviews
                    .Where(r => r.MovieId == movieId)
                    .ToListAsync();
                if (reviewsForMovie.Any())
                {
                    double newAverageRating = reviewsForMovie.Average(r => r.Rating);
                    movie.IMDB = Math.Round(newAverageRating, 1);
                }
                else
                {
                    movie.IMDB = 0;
                }
                _context.Movies.Update(movie);
                await _context.SaveChangesAsync();
            }
            return NoContent();
        }
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<UserReview>>> GetReviewsByUserId(string userId)
        {
            var reviews = await _context.UserReviews
                .Where(r => r.UserId == userId)
                .ToListAsync();
            if (reviews == null || !reviews.Any())
            {
                return NotFound("Bu kullanıcı için hiç inceleme bulunamadı.");
            }
            return Ok(reviews);

        }
        [HttpGet("movie/{movieId}/user/{userId}")]
        public async Task<ActionResult<UserReview>> GetReviewByMovieAndUser(int movieId, string userId)
        {
            var review = await _context.UserReviews
                .FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);
            if (review == null)
            {
                return NotFound("Bu film ve kullanıcı için inceleme bulunamadı.");
            }
            return Ok(review);
        }

    }
}

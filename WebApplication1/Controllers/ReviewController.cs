using Microsoft.AspNetCore.Mvc;
using WebApplication1.Entitites;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DbContexts;
using WebApplication1.DbContexts.MovieRecData;
using WebApplication1.Dto;

namespace WebApplication1.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewRepository _reviewRepository;
        private readonly MovieRepository _movieRepository;
        public ReviewController(AllDbContext context)
        {
            _reviewRepository = new ReviewRepository(context);
            _movieRepository = new MovieRepository(context);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetAllReviews()
        {
            var reviews = await _reviewRepository.GetReviewsAsync();
            List<ReviewDTO> reviewDTOs = new List<ReviewDTO>();
            if (reviews == null || !reviews.Any())
            {
                return NotFound("Hiç kullanıcı incelemesi bulunamadı.");
            }
            foreach (var review in reviews)
            {
                reviewDTOs.Add(new ReviewDTO(review));
            }
            return Ok(reviewDTOs);

        }

        [HttpGet("{movieId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByMovieId(int movieId)
        {
            var reviews = await _reviewRepository.GetReviewByMovieIdAsync(movieId);
            List<ReviewDTO> reviewDTOs = new List<ReviewDTO>();
            if (reviews == null || !reviews.Any())
            {
                return NotFound("Bu film için hiç kullanıcı yorumu bulunamadı.");
            }
            foreach (var review in reviews)
            {
                reviewDTOs.Add(new ReviewDTO(review));
            }
            return Ok(reviewDTOs);
        }
        [HttpPost]
        public async Task<ActionResult<Review>> AddReview([FromBody] Review newReview)
        {
            if (newReview == null)
            {
                return BadRequest("İnceleme bilgileri boş olamaz.");
            }

            var existingReview = await _reviewRepository.GetReviewsByUserAndMovieIdAsync(newReview.UserId, newReview.MovieId);

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

            await _reviewRepository.Create(newReview);

            var movie = await _movieRepository.GetMovieByIdAsync(newReview.MovieId);
            var reviewsForMovie = await _reviewRepository.GetReviewByMovieIdAsync(newReview.MovieId);
            if (reviewsForMovie.Any())
            {
                double newAverageRating = reviewsForMovie.Average(r => r.Rating);
                movie.IMDB = Math.Round(newAverageRating, 1);
            }
            else
            {
                movie.IMDB = newReview.Rating;
            }
            await _movieRepository.Update(movie);
            ReviewDTO reviewDTO = new ReviewDTO(newReview);
            return CreatedAtAction(nameof(GetReviewsByMovieId), new { movieId = newReview.MovieId }, reviewDTO);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] Review updatedReview)
        {
            if (id != updatedReview.Id)
            {
                return BadRequest("İnceleme ID'leri değiştirilemez.");
            }
            var Review = await _reviewRepository.GetReviewByIdAsync(id);
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
            await _reviewRepository.Update(Review);

            var movie = await _movieRepository.GetMovieByIdAsync(Review.MovieId);
            var reviewsForMovie = await _reviewRepository.GetReviewByMovieIdAsync(Review.MovieId);

            if (reviewsForMovie!= null)
            {
                double newAverageRating = reviewsForMovie.Average(r => r.Rating);
                movie.IMDB = Math.Round(newAverageRating, 1);
            }
            else
            {
                movie.IMDB = Review.Rating;
            }
            await _movieRepository.Update(movie);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            if (review == null)
            {
                return NotFound("Silinecek inceleme bulunamadı.");
            }
            var movieId = review.MovieId;
            await _reviewRepository.Delete(id);

            var movie = await _movieRepository.GetMovieByIdAsync(movieId);
            if (movie != null)
            {
                var reviewsForMovie = await _reviewRepository.GetReviewByMovieIdAsync(movieId);
                if (reviewsForMovie.Any())
                {
                    double newAverageRating = reviewsForMovie.Average(r => r.Rating);
                    movie.IMDB = Math.Round(newAverageRating, 1);
                }
                else
                {
                    movie.IMDB = 0;
                }
                await _movieRepository.Update(movie);
            }
            return NoContent();
        }
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByUserId(string userId)
        {
            var reviews = await _reviewRepository.GetReviewsByUserIdAsync(userId);
            List<ReviewDTO> reviewDTOs = new List<ReviewDTO>();
            if (reviews == null || !reviews.Any())
            {
                return NotFound("Bu kullanıcı için hiç inceleme bulunamadı.");
            }
            foreach (var review in reviews)
            {
                reviewDTOs.Add(new ReviewDTO(review));
            }
            return Ok(reviewDTOs);

        }
        [HttpGet("movie/{movieId}/user/{userId}")]
        public async Task<ActionResult<Review>> GetReviewByMovieAndUser(int movieId, string userId)
        {
            var review = await _reviewRepository.GetReviewsByUserAndMovieIdAsync(userId, movieId);
            if (review == null)
            {
                return NotFound("Bu film ve kullanıcı için inceleme bulunamadı.");
            }
            ReviewDTO reviewDTO = new ReviewDTO(review.FirstOrDefault());
            return Ok(reviewDTO);
        }

    }
}

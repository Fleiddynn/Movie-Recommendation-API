using Microsoft.AspNetCore.Mvc;
using WebApplication1.Entitites;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DbContexts;
using WebApplication1.DbContexts.MovieRecData;
using WebApplication1.Dto;
using WebApplication1.DbContexts.UserData;

namespace WebApplication1.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewRepository _reviewRepository;
        private readonly MovieRepository _movieRepository;
        private readonly UserRepository _userRepository;
        public ReviewController(AllDbContext context)
        {
            _reviewRepository = new ReviewRepository(context);
            _movieRepository = new MovieRepository(context);
            _userRepository = new UserRepository(context);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetAllReviews()
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
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetReviewsByMovieId(Guid movieId)
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
        public async Task<ActionResult<Review>> AddReview([FromBody] ReviewAddDTO newReviewDto)
        {
            if (newReviewDto == null)
            {
                return BadRequest("İnceleme bilgileri boş olamaz.");
            }

            var hasExistingReview = await _reviewRepository
                .GetReviewsByUserAndMovieIdAsync(newReviewDto.UserId, newReviewDto.MovieId)
                .ContinueWith(t => t.Result.Any());

            if (hasExistingReview)
            {
                return BadRequest("Bu kullanıcı zaten bu filmi incelemiş.");
            }

            if (newReviewDto.Rating < 0 || newReviewDto.Rating > 10)
            {
                return BadRequest("Verilen puan 0 ila 10 arasında olmalıdır.");
            }

            var newReview = new Review
            {
                Id = Guid.NewGuid(),
                UserId = newReviewDto.UserId,
                User = await _userRepository.GetUserByIdAsync(newReviewDto.UserId),
                MovieId = newReviewDto.MovieId,
                Movie = await _movieRepository.GetMovieByIdAsync(newReviewDto.MovieId),
                Note = newReviewDto.Note,
                Rating = newReviewDto.Rating,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

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
        public async Task<IActionResult> UpdateReview(Guid id, [FromBody] ReviewAddDTO updatedReview)
        {
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
            Review.UpdatedAt = DateTime.UtcNow;
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
        public async Task<IActionResult> DeleteReview(Guid id)
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
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetReviewsByUserId(Guid userId)
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
        public async Task<ActionResult<ReviewDTO>> GetReviewByMovieAndUser(Guid movieId, Guid userId)
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

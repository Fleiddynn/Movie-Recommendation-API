using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using WebApplication1.Entitites;
using WebApplication1.DbContexts.MovieRecData;
using WebApplication1.DbContexts;

namespace WebApplication1.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly MovieRepository _movieRepository;

        public MoviesController(AllDbContext context)
        {
            _movieRepository = new MovieRepository(context);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            var movies = await _movieRepository.GetMoviesAsync();
            List<MovieDTO> movieDTOs = new List<MovieDTO>();
            if (movies == null || !movies.Any())
            {
                return NotFound("Hiç film bulunamadı.");
            }
            else
            {
                foreach (var movie in movies){
                    movieDTOs.Add(new MovieDTO(movie));
                }
            }
            return Ok(movieDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDTO>> GetMovie(int id)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(id);
            MovieDTO movieDTO = new MovieDTO(movie);

            if (movieDTO == null)
            {
                return NotFound($"Aradığınız film bulunamadı.");
            }

            return Ok(movieDTO);
        }

        [HttpPost]
        public async Task<ActionResult<Movie>> AddMovie([FromBody] Movie newMovie)
        {
            _movieRepository.Create(newMovie);
            await _movieRepository.Update(newMovie);

            return CreatedAtAction(nameof(GetMovie), new { id = newMovie.Id }, newMovie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] Movie updatedMovie)
        {
            try
            {
                var movie = await _movieRepository.GetMovieByIdAsync(id);
                if (movie == null)
                {
                    return NotFound($"Aradığınız film bulunamadı.");
                }

                movie.Title = updatedMovie.Title;
                movie.Description = updatedMovie.Description;
                movie.Duration = updatedMovie.Duration;
                movie.Director = updatedMovie.Director;
                movie.Categories = updatedMovie.Categories;
                movie.Length = updatedMovie.Length;
                movie.ReleaseDate = updatedMovie.ReleaseDate;
                movie.UpdatedAt = DateTime.UtcNow;

                await _movieRepository.Update(movie);
                return Ok(movie);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _movieRepository.GetMovieByIdAsync(id).ContinueWith(t => t.Result != null))
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                return BadRequest($"Güncelleme sırasında bir hata oluştu: {ex.Message}");
            }

            return NoContent();
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateMovie(int id, [FromBody] JsonPatchDocument<Movie> patchDoc)
        {
            if (patchDoc.Operations.Any(op => op.path.ToLower() == "/imdb"))
            {
                return BadRequest("Film puanı güncellenemez.");
            }

            var movieToPatch = await _movieRepository.GetMovieByIdAsync(id);

            if (movieToPatch == null)
            {
                return NotFound("Aradığınız film bulunamadı.");
            }

            patchDoc.ApplyTo(movieToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _movieRepository.Update(movieToPatch);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movieToDelete = await _movieRepository.GetMovieByIdAsync(id);
            if (movieToDelete == null)  
            {
                return NotFound($"Silmeye çalıştığınız film bulunamadı.");
            }

            var deletedMovie = await _movieRepository.Delete(id); 

            if (deletedMovie == null)
            {
                return NotFound($"Silmeye çalıştığınız film bulunamadı.");
            }

            return Ok();
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesByCategory(Guid categoryId)
        {
            var movies = await _movieRepository.GetMoviesByCategoryAsync(categoryId);
            if (movies == null || !movies.Any())
            {
                return NotFound($"Bu kategoriye ait film bulunamadı.");
            }
            List<MovieDTO> movieDTOs = new List<MovieDTO>();
            foreach (var movie in movies)
            {
                movieDTOs.Add(new MovieDTO(movie));
            }
            return Ok(movieDTOs);
        }

        [HttpPost("{movieId}/addcategory/{categoryId}")]
        public async Task<IActionResult> AddCategoryToMovie(int movieId, Guid categoryId)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(movieId);
            if (movie == null)
            {
                return NotFound($"Aradığınız film bulunamadı.");
            }
            if (movie.Categories.Contains(categoryId))
            {
                return BadRequest("Bu kategori zaten filme eklenmiş.");
            }
            movie.Categories.Add(categoryId);
            await _movieRepository.Update(movie);
            return Ok(new { message = "Kategori filme başarıyla eklendi." });
        }
        [HttpGet("category/{categoryId}/page/{pageNumber}/size/{pageSize}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesByCategoryWithPagination(Guid categoryId, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Sayfa numarası ve sayfa boyutu 0'dan büyük olmalıdır.");
            }
            var movies = await _movieRepository.GetMoviesByCategoryAsync(categoryId, pageNumber, pageSize);
            if (movies == null || !movies.Any())
            {
                return NotFound($"Bu kategoriye ait film bulunamadı.");
            }
            List<MovieDTO> movieDTOs = new List<MovieDTO>();
            foreach (var movie in movies)
            {
                movieDTOs.Add(new MovieDTO(movie));
            }
            return Ok(movieDTOs);
        }
    }
}
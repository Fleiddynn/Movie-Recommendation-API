using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using WebApplication1.MovieRecData;
using WebApplication1;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly MDbContext _context;

        public MoviesController(MDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieApi>>> GetMovies()
        {
            return Ok(await _context.Movies.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieApi>> GetMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound($"Aradığınız film bulunamadı.");
            }

            return Ok(movie);
        }

        [HttpPost]
        public async Task<ActionResult<MovieApi>> AddMovie([FromBody] MovieApi newMovie)
        {
            _context.Movies.Add(newMovie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovie), new { id = newMovie.Id }, newMovie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] MovieApi updatedMovie)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound($"Aradığınız film bulunamadı.");
            }

            movie.Title = updatedMovie.Title;
            movie.Description = updatedMovie.Description;
            movie.Cast = updatedMovie.Cast;
            movie.Director = updatedMovie.Director;
            movie.Categories = updatedMovie.Categories;
            movie.IMDB = updatedMovie.IMDB;
            movie.Length = updatedMovie.Length;
            movie.ReleaseDate = updatedMovie.ReleaseDate;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Movies.AnyAsync(e => e.Id == id))
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateMovie(int id, [FromBody] JsonPatchDocument<MovieApi> patchDoc)
        {
            var movieToPatch = await _context.Movies.FindAsync(id);

            if (movieToPatch == null)
            {
                return NotFound($"Aradığınız film bulunamadı.");
            }

            patchDoc.ApplyTo(movieToPatch, ModelState);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movieToDelete = await _context.Movies.FindAsync(id);
            if (movieToDelete == null)
            {
                return NotFound($"Silmeye çalıştığınız film bulunamadı.");
            }

            _context.Movies.Remove(movieToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
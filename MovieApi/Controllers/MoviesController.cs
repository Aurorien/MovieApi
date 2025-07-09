using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models.DTOs.MovieDtos;
using MovieApi.Models.Entities;

namespace MovieApi.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieApiContext _context;

        public MoviesController(MovieApiContext context)
        {
            _context = context;
        }


        // GET: api/movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            var movieDtos = await _context.Movie
                .Select(m => new MovieDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Year = m.Year,
                    Genre = m.Genre,
                    DurationInMinutes = m.DurationInMinutes,
                    MovieDetailsLanguage = m.MovieDetails.Language
                })
                .ToListAsync();

            return Ok(movieDtos);
        }


        // GET: api/movies/5
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MovieDto>> GetMovie([FromRoute] Guid id)
        {
            var movieDto = await _context.Movie
                .Where(m => m.Id == id)
                .Select(m => new MovieDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Year = m.Year,
                    Genre = m.Genre,
                    DurationInMinutes = m.DurationInMinutes,
                    MovieDetailsLanguage = m.MovieDetails.Language
                })
                .FirstOrDefaultAsync();

            if (movieDto == null)
            {
                return NotFound();
            }

            return Ok(movieDto);
        }


        // GET: api/movies/5/detailed
        [HttpGet("{id:guid}/detailed")]
        public async Task<ActionResult<MovieDetailedDto>> GetMovieDetailed([FromRoute] Guid id)
        {
            var movieDetailed = await _context.Movie
                .Where(m => m.Id == id)
                .Include(m => m.MovieDetails)
                .Select(m => new MovieDetailedDto
                {
                    Title = m.Title,
                    Year = m.Year,
                    Genre = m.Genre,
                    DurationInMinutes = m.DurationInMinutes,
                    Synopsis = m.MovieDetails.Synopsis,
                    Language = m.MovieDetails.Language,
                    Budget = m.MovieDetails.Budget
                })
                .FirstOrDefaultAsync();


            if (movieDetailed is null) return NotFound();

            return Ok(movieDetailed);
        }


        // POST: api/movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovieDto>> PostMovie([FromBody] MovieCreateDto createMovieDto)
        {
            var movie = new Movie
            {
                Title = createMovieDto.Title,
                Year = createMovieDto.Year,
                Genre = createMovieDto.Genre,
                DurationInMinutes = createMovieDto.DurationInMinutes,
                MovieDetails = new MovieDetails
                {
                    Synopsis = createMovieDto.Synopsis,
                    Language = createMovieDto.Language,
                    Budget = createMovieDto.Budget
                }

            };

            _context.Movie.Add(movie);
            await _context.SaveChangesAsync();

            var movieDto = new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre,
                DurationInMinutes = movie.DurationInMinutes,
                MovieDetailsLanguage = movie.MovieDetails.Language
            };

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movieDto);
        }


        // PUT: api/movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutMovie([FromRoute] Guid id, [FromBody] MoviePutUpdateDto updateMovieDto)
        {
            var movie = await _context.Movie
                   .Include(m => m.MovieDetails)
                   .Include(m => m.Actors)
                   .FirstOrDefaultAsync(m => m.Id == id);

            if (movie is null) return NotFound();

            movie.Title = updateMovieDto.Title;
            movie.Year = updateMovieDto.Year;
            movie.Genre = updateMovieDto.Genre;
            movie.DurationInMinutes = updateMovieDto.DurationInMinutes;
            movie.MovieDetails.Synopsis = updateMovieDto.Synopsis;
            movie.MovieDetails.Language = updateMovieDto.Language;
            movie.MovieDetails.Budget = updateMovieDto.Budget;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE: api/movies/5
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteMovie([FromRoute] Guid id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(Guid id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models.DTOs.ActorDtos;
using MovieApi.Models.Entities;

namespace MovieApi.Controllers
{
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly MovieApiContext _context;

        public ActorsController(MovieApiContext context)
        {
            _context = context;
        }


        // GET: api/actors
        [HttpGet("api/actors")]
        public async Task<ActionResult<IEnumerable<ActorDto>>> GetActor()
        {
            var actorDtos = await _context.Actor
                .Select(a => new ActorDto
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    BirthYear = a.BirthYear
                })
                .ToListAsync();

            return Ok(actorDtos);
        }


        // GET: api/actors/5
        [HttpGet("api/actors{id:guid}")]
        public async Task<ActionResult<Actor>> GetActor([FromRoute] Guid id)
        {
            var actorDto = await _context.Actor
                .Where(a => a.Id == id)
                .Select(a => new ActorDto
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    BirthYear = a.BirthYear
                })
                .FirstOrDefaultAsync();

            if (actorDto == null)
            {
                return NotFound();
            }

            return Ok(actorDto);
        }


        // POST: api/actors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("api/actors")]
        public async Task<ActionResult<ActorDto>> PostActor([FromBody] ActorCreateDto createActorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var actor = new Actor
            {
                FirstName = createActorDto.FirstName,
                LastName = createActorDto.LastName,
                BirthYear = createActorDto.BirthYear,
            };

            _context.Actor.Add(actor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActor", new { id = actor.Id }, actor);
        }


        // POST: api/movies/5/actors/1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("api/movies/{movieId}/actors/{actorId}")]
        public async Task<IActionResult> PostActorToMovie([FromRoute] Guid movieId, [FromRoute] Guid actorId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movie = await _context.Movie.FirstOrDefaultAsync(m => m.Id == movieId);
            if (movie == null) return NotFound("Movie not found");

            var actor = await _context.Actor.FirstOrDefaultAsync(m => m.Id == actorId);
            if (actor == null) return NotFound("Actor not found");

            if (movie.Actors.Any(a => a.Id == actorId))
                return BadRequest("Actor is already in this movie");

            movie.Actors.Add(actor);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // PUT: api/actors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("api/actors{id}")]
        public async Task<IActionResult> PutActor([FromRoute] Guid id, [FromBody] ActorPutUpdateDto updateActorDto)
        {
            var actor = await _context.Actor.FirstOrDefaultAsync(a => a.Id == id);

            if (actor is null) return NotFound();

            actor.FirstName = updateActorDto.FirstName;
            actor.LastName = updateActorDto.LastName;
            actor.BirthYear = updateActorDto.BirthYear;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActorExists(id))
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


        private bool ActorExists(Guid id)
        {
            return _context.Actor.Any(e => e.Id == id);
        }
    }
}

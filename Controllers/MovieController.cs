using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using movies_api.Data;
using movies_api.Data.Dtos;
using movies_api.Models;

namespace movies_api.Controllers;
[ApiController]
[Route("movies")]
public class MovieController : ControllerBase
{
    private MovieDbContext _context;
    private IMapper _mapper;

    public MovieController(MovieDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public IEnumerable<Movie> GetMovies([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        return _context.Movies.Skip(skip).Take(take);
    }

    [HttpGet("{id}")]
    public IActionResult GetMovieById([FromRoute] string id)
    {
        if (!Guid.TryParse(id, out var parseId))
        {
            return BadRequest("Invalid ID");
        }

        var movie = _context.Movies.FirstOrDefault(movie => movie.Id == parseId);

        if (movie == null)
        {
            return NotFound();
        }

        return Ok(movie);
    }

    [HttpPost]
    public IActionResult CreateMovie(
        [FromBody] CreateMovieDto movieDto)
    {
        Movie movie = _mapper.Map<Movie>(movieDto);

        _context.Movies.Add(movie);
        _context.SaveChanges();

        return CreatedAtAction(
            nameof(GetMovieById), new { movie.Id }, movie);
    }
}

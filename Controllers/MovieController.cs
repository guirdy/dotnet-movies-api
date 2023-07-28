using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
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
    public IEnumerable<ReadMovieDto> GetMovies([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        return _mapper.Map<List<ReadMovieDto>>(_context.Movies.Skip(skip).Take(take));
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

        var movieDto = _mapper.Map<ReadMovieDto>(movie);

        return Ok(movieDto);
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

    [HttpPut("{id}")]
    public IActionResult UpdateMovie(
        [FromRoute] string id,
        [FromBody] UpdateMovieDto movieDto)
    {
        if (!Guid.TryParse(id, out var parseId))
        {
            return BadRequest("Invalid ID");
        }

        Movie? movie = _context.Movies
            .FirstOrDefault(movie => movie.Id == parseId);

        if (movie == null )
        {
            return BadRequest();
        }

        // Mapping movieDto to movie
        _mapper.Map(movieDto, movie);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult PartialUpdateMovie(
        [FromRoute] string id,
        [FromBody] JsonPatchDocument<UpdateMovieDto> patch)
    {
        if (!Guid.TryParse(id, out var parseId))
        {
            return BadRequest("Invalid ID");
        }

        Movie? movie = _context.Movies
            .FirstOrDefault(movie => movie.Id == parseId);

        if (movie == null)
        {
            return BadRequest();
        }

        var movieToUpdate = _mapper.Map<UpdateMovieDto>(movie);

        patch.ApplyTo(movieToUpdate, ModelState);

        if(!TryValidateModel(movieToUpdate))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(movieToUpdate, movie);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteMovieById([FromRoute] string id)
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

        _context.Remove(movie);
        _context.SaveChanges();

        return NoContent();
    }
}

using System.ComponentModel.DataAnnotations;

namespace movies_api.Data.Dtos;

public class ReadMovieDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Gender { get; set; }
    public int Duration { get; set; }
}

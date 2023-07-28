using System.ComponentModel.DataAnnotations;

namespace movies_api.Models;

public class Movie
{
    [Key]
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required(ErrorMessage = "The movie title is required")]
    public string Title { get; set; }
    [Required(ErrorMessage = "The movie genre is required")]
    [MaxLength(50, ErrorMessage = "Genre length cannot be grater than 50 characters")]
    public string Gender { get; set; }
    [Required(ErrorMessage = "The movie title is required")]
    [Range(70, 600, ErrorMessage = "The duration movie must be between 70 and 600 minutes")]
    public int Duration { get; set; }
}

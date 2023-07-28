using Microsoft.EntityFrameworkCore;
using movies_api.Models;

namespace movies_api.Data;

public class MovieDbContext : DbContext
{
    public MovieDbContext(DbContextOptions<MovieDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<Movie> Movies { get; set; }
}

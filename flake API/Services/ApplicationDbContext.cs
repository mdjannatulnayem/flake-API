using flake_API.Models;
using Microsoft.EntityFrameworkCore;

namespace flake_API.Services;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options){}

    public DbSet<LocationModel> Location { get; set; }
    public DbSet<WeatherDataModel> Weather { get; set; }
}

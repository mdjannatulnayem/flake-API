using flake_API.Models;
using Microsoft.EntityFrameworkCore;

namespace flake_API.Services;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options){}

    public DbSet<LocationModel> Location { get; set; }
    public DbSet<WeatherDataModel> Weather { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<LocationModel>().HasData(
            new LocationModel
            {
                State = "dhaka"
            },
            new LocationModel
            {
                State = "sylhet"
            },
            new LocationModel
            {
                State = "mymensingh"
            },
            new LocationModel
            {
                State = "rajshahi"
            },
            new LocationModel
            {
                State = "dinajpur"
            },
            new LocationModel
            {
                State = "rangpur"
            },
            new LocationModel
            {
                State = "barisal"
            },
            new LocationModel
            {
                State = "chittagong"
            }
            );
    }
}

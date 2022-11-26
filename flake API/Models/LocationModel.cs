using System.ComponentModel.DataAnnotations;

namespace flake_API.Models;

public class LocationModel
{
    [Key]
    public int LocationId { get; set; }
    [Required]
    public string? Location { get; set; }
}

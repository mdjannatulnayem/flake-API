using System.ComponentModel
    .DataAnnotations;

namespace flake_API.Models;

public class LocationModel
{
    [Key]
    public string State { get; set; }
}

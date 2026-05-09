using System.ComponentModel.DataAnnotations;

namespace APBD4.Models;

public class Room
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is  required")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Building Code is  required")]
    public string BuildingCode { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be higher than 0")]
    public int Capacity { get; set; }
    
    public bool HasProjector { get; set; }
    
    public bool IsActive { get; set; }
}
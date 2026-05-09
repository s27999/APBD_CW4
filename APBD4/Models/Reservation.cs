using System.ComponentModel.DataAnnotations;

namespace APBD4.Models;

public enum Status
{
    planned,
    confirmed,
    cancelled
}

public class Reservation : IValidatableObject
{
    public int Id { get; set; }
    
    public int RoomId { get; set; }
    
    [Required(ErrorMessage = "Organizer Name is required")]
    public string OrganizerName { get; set; }
    
    [Required(ErrorMessage = "Topic is required")]
    public string Topic { get; set; }
    
    public DateOnly Date { get; set; }
    
    public TimeOnly StartTime { get; set; }
    
    public TimeOnly EndTime { get; set; }
    
    public Status Status { get; set; }
    
    
    //Validate żeby nie sprawdzać tego bezpośrednio w kontrolerze
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndTime <= StartTime)
        {
            yield return new ValidationResult("EndTime must be later than StartTime.", new[] { nameof(EndTime) });
        }
    }
}
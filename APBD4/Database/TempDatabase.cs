using APBD4.Models;

namespace APBD4.Database;

public class TempDatabase
{
    public static List<Room> Rooms { get; set; } = new List<Room>()
    {
        new Room { Id = 1, Name = "Sala 101", BuildingCode = "A", Capacity = 30, HasProjector = true, IsActive = true },
        new Room { Id = 2, Name = "Sala 102", BuildingCode = "A", Capacity = 15, HasProjector = false, IsActive = true },
        new Room { Id = 3, Name = "Lab 204", BuildingCode = "B", Capacity = 24, HasProjector = true, IsActive = true },
        new Room { Id = 4, Name = "Aula Główna", BuildingCode = "C", Capacity = 200, HasProjector = true, IsActive = true },
        new Room { Id = 5, Name = "Sala 301", BuildingCode = "B", Capacity = 10, HasProjector = true, IsActive = false }
    };
    
    public static List<Reservation> Reservations { get; set; } = new List<Reservation>
    {
        new Reservation 
        { 
            Id = 1, RoomId = 1, OrganizerName = "Jan Kowalski", Topic = "Wstęp do C#", 
            Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(9, 30), Status = Status.confirmed 
        },
        new Reservation 
        { 
            Id = 2, RoomId = 3, OrganizerName = "Anna Nowak", Topic = "Warsztaty z HTTP i REST", 
            Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(12, 30), Status = Status.confirmed 
        },
        new Reservation 
        { 
            Id = 3, RoomId = 4, OrganizerName = "Piotr Wiśniewski", Topic = "Wykład gościnny", 
            Date = new DateOnly(2026, 5, 12), StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(16, 0), Status = Status.planned 
        },
        new Reservation 
        { 
            Id = 4, RoomId = 2, OrganizerName = "Katarzyna Wójcik", Topic = "Konsultacje", 
            Date = new DateOnly(2026, 5, 15), StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(10, 0), Status = Status.cancelled 
        },
        new Reservation 
        { 
            Id = 5, RoomId = 1, OrganizerName = "Jan Kowalski", Topic = "Zaawansowany C#", 
            Date = new DateOnly(2026, 5, 16), StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(9, 30), Status = Status.planned 
        }
    };
}
using APBD4.Models;
using Microsoft.AspNetCore.Mvc;

namespace APBD4.Controllers;


[ApiController]
[Route("api/reservations")]
public class ReservationController : ControllerBase
{
    private List<Room> _rooms = Database.TempDatabase.Rooms;
    private List<Reservation> _reservations = Database.TempDatabase.Reservations;
    
    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> GetReservations(
        [FromQuery] DateOnly? date,
        [FromQuery] Status? status,
        [FromQuery] int? roomId)
    {

        var reservations = _reservations.AsEnumerable();
        
        if (date.HasValue)
        {
            reservations = reservations.Where(r => r.Date == date);
        }

        if (status.HasValue)
        {
            reservations = reservations.Where(r => r.Status == status);
        }

        if (roomId.HasValue)
        {
            reservations = reservations.Where(r => r.RoomId == roomId);
        }

        return Ok(reservations);
    }

    [HttpGet("{id}")]
    public ActionResult<Reservation> GetReservation(int id)
    {
        var reservation = _reservations.First(r => r.Id == id);
        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult<Reservation> AddReservation(Reservation reservation)
    {
        var room = _rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
        
        if (room is null)
        {
            return BadRequest("No Room Found");
        }

        if (!room.IsActive)
        {
            return BadRequest("Room is not active");
        }

        if (_reservations.Any(r => r.StartTime >= reservation.StartTime && r.EndTime <= reservation.EndTime))
        {
            return BadRequest("Reservation is already active");
        }
        
        
        reservation.Id = _reservations.Any() ? _reservations.Max(r => r.Id) + 1 : 1;
        _reservations.Add(reservation);
        return Ok(reservation);
    }

    [HttpPut("{id}")]
    public ActionResult<Reservation> UpdateReservation(int id, Reservation reservation)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var updatedReservation = _reservations.First(r => r.Id == id);
        
        updatedReservation.RoomId = reservation.RoomId;
        updatedReservation.OrganizerName = reservation.OrganizerName;
        updatedReservation.Topic = reservation.Topic;
        updatedReservation.Date = reservation.Date;
        updatedReservation.StartTime = reservation.StartTime;
        updatedReservation.EndTime = reservation.EndTime;
        updatedReservation.Status = reservation.Status;
        
        
    }
}
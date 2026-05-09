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
        var reservation = _reservations.FirstOrDefault(r => r.Id == id);

        if (reservation is null)
        {
            return NotFound();
        }
        
        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult<Reservation> AddReservation(Reservation reservation)
    {
        var room = _rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
        
        bool isConflict = _reservations.Any(r => 
            r.RoomId == reservation.RoomId && 
            r.Date == reservation.Date && 
            r.StartTime < reservation.EndTime && 
            r.EndTime > reservation.StartTime);
        
        if (room is null)
        {
            return BadRequest("No Room Found");
        }

        if (!room.IsActive)
        {
            return BadRequest("Room is not active");
        }

        if (isConflict)
        {
            return Conflict("Reservation is already active");
        }
        
        
        reservation.Id = _reservations.Any() ? _reservations.Max(r => r.Id) + 1 : 1;
        _reservations.Add(reservation);
        return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
    }

    [HttpPut("{id}")]
    public ActionResult<Reservation> UpdateReservation(int id, Reservation reservation)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var updatedReservation = _reservations.FirstOrDefault(r => r.Id == id);

        if (updatedReservation is null)
        {
            return NotFound();
        }
        
        updatedReservation.RoomId = reservation.RoomId;
        updatedReservation.OrganizerName = reservation.OrganizerName;
        updatedReservation.Topic = reservation.Topic;
        updatedReservation.Date = reservation.Date;
        updatedReservation.StartTime = reservation.StartTime;
        updatedReservation.EndTime = reservation.EndTime;
        updatedReservation.Status = reservation.Status;
        
        return Ok(updatedReservation);
    }

    [HttpDelete("{id}")]
    public ActionResult<Reservation> DeleteReservation(int id)
    {
        var reservationToDelete = _reservations.FirstOrDefault(r => r.Id == id);

        if (reservationToDelete is null)
        {
            return NotFound();
        }
        
        _reservations.Remove(reservationToDelete);
        return NoContent();
    }
}
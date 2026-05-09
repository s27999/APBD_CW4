using APBD4.Models;
using Microsoft.AspNetCore.Mvc;

namespace APBD4.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomController : ControllerBase
{
    private List<Room> _rooms = Database.TempDatabase.Rooms;
    private List<Reservation> _reservations = Database.TempDatabase.Reservations;
    
    [HttpGet]
    public ActionResult<IEnumerable<Room>> GetRooms(
        [FromQuery] int? minCapacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly)
    {
        var rooms = _rooms.AsEnumerable();

        if (minCapacity.HasValue)
        {
            rooms = rooms.Where(r => r.Capacity >= minCapacity.Value);
        }

        if (hasProjector.HasValue)
        {
            rooms =  rooms.Where(r => r.HasProjector == hasProjector.Value);
        }

        if (activeOnly.HasValue)
        {
            rooms = rooms.Where(r => r.IsActive == activeOnly.Value);
        }
        
        return Ok(rooms);
    }

    [HttpGet("{id}")]
    public ActionResult<Room> GetRoom(int id)
    {
        
        
        var room = _rooms.FirstOrDefault(r => r.Id == id);
        
        return room is null ? NotFound() : Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public ActionResult<IEnumerable<Room>> GetRoomsByBuilding([FromRoute] string buildingCode)
    {
        var roomsByBuilding = _rooms.Where(r => r.BuildingCode == buildingCode);
        
        return roomsByBuilding.Any() ? Ok(roomsByBuilding) : NotFound();
    }

    [HttpPost]
    public ActionResult<Room> CreateRoom(Room room)
    {
        room.Id = _rooms.Any() ? _rooms.Max(r => r.Id) +1 : 1;
        
        _rooms.Add(room);
        
        return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
    }

    [HttpPut("{id}")]
    public ActionResult<Room> UpdateRoom(int id, Room room)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var updatedRoom = _rooms.FirstOrDefault(r => r.Id == id);

        if (updatedRoom is null)
        {
            return NotFound();
        }
        
        updatedRoom.BuildingCode = room.BuildingCode;
        updatedRoom.Name = room.Name;
        updatedRoom.Capacity = room.Capacity;
        updatedRoom.HasProjector = room.HasProjector;
        updatedRoom.IsActive = room.IsActive;

        return Ok(updatedRoom);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteRoom(int id)
    {
        var roomToDelete = _rooms.FirstOrDefault(r => r.Id == id);

        if (roomToDelete is null)
        {
            return NotFound();
        }

        if (_reservations.Any(r => r.RoomId == id))
        {
            return Conflict("You cannot remove a room with a reservation");
        }
        
        _rooms.Remove(roomToDelete);
        return NoContent();
    }
}
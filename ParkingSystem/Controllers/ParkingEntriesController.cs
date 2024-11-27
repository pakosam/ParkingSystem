using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingSystem.Data;
using ParkingSystem.Entities;

namespace ParkingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingEntriesController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public List<int> Items { get; set; }
        public ParkingEntriesController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost("{ParkingId}/entries")]
        public async Task<ActionResult<List<ParkingEntry>>> AddParkingEntry([FromRoute] int ParkingId, [FromBody] ParkingEntry parkingEntry)
        {
            var parking = await _dataContext.Parkings.FindAsync(ParkingId);

            if (parking == null)
            {
                return NotFound($"Parking with ID {ParkingId} not found.");
            }

            parkingEntry.ParkingId = ParkingId;

            _dataContext.ParkingEntries.Add(parkingEntry);
            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.ParkingEntries.ToListAsync());
        }

        [HttpPut("{parkingId}/entries")]
        public async Task<ActionResult<List<ParkingEntry>>> UpdateParkingEntry([FromRoute] int parkingId, [FromBody] ParkingEntry updatedParkingEntry)
        {
            var parking = await _dataContext.Parkings.FindAsync(parkingId);

            if (parking == null)
            {
                return NotFound($"Parking with ID {parkingId} not found.");
            }

            var dbParkingEntry = await _dataContext.ParkingEntries.FindAsync(updatedParkingEntry.Id);

            if (dbParkingEntry == null)
                return NotFound("Parking entry not found");

            dbParkingEntry.RegistrationPlate = updatedParkingEntry.RegistrationPlate;
            dbParkingEntry.TicketTakeover = updatedParkingEntry.TicketTakeover;
            dbParkingEntry.TicketExpiration = updatedParkingEntry.TicketExpiration;
            dbParkingEntry.ParkingId = parkingId;

            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.ParkingEntries.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult<List<ParkingEntry>>> GetAllParkingEntries()
        {
            var parkingEntries = await _dataContext.ParkingEntries.ToListAsync();

            return Ok(parkingEntries);
        }

        [HttpGet("{id}/entries")]
        public async Task<ActionResult<ParkingEntry>> GetParkingEntry(int id)
        {
            var parkingEntry = await _dataContext.ParkingEntries.FindAsync(id);

            if (parkingEntry == null)
                return NotFound("Parking entry not found.");

            await _dataContext.SaveChangesAsync();

            return Ok(parkingEntry);
        }

        [HttpDelete]
        public async Task<ActionResult<List<ParkingEntry>>> DeleteParkingEntry(int id)
        {
            var dbParkingEntry = await _dataContext.ParkingEntries.FindAsync(id);

            if (dbParkingEntry == null)
                return NotFound("Parking entry not found.");

            _dataContext.ParkingEntries.Remove(dbParkingEntry);
            await _dataContext.SaveChangesAsync();
            
            return Ok(await _dataContext.ParkingEntries.ToListAsync());
        }
    }
}
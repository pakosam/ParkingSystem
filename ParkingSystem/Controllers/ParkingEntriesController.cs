using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingSystem.Data;
using ParkingSystem.DTOs;
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
        //[Authorize]
        public async Task<ActionResult<List<ParkingEntryDto>>> AddParkingEntry([FromRoute] int ParkingId, [FromBody] CreateParkingEntryDto createParkingEntryDto)
        {
            var parking = await _dataContext.Parkings.FindAsync(ParkingId);

            if (parking == null)
                return NotFound($"Parking with ID {ParkingId} not found.");

            var parkingEntry = new ParkingEntry
            {
                RegistrationPlate = createParkingEntryDto.RegistrationPlate,
                TicketTakeover = createParkingEntryDto.TicketTakeover,
                ParkingId = ParkingId
            }; 

            _dataContext.ParkingEntries.Add(parkingEntry);
            await _dataContext.SaveChangesAsync();

            var parkingEntries = await _dataContext.ParkingEntries
                .Where(p => p.ParkingId == ParkingId)
                .Select(p => new ParkingEntryDto
                {
                     Id = p.Id,
                     RegistrationPlate = p.RegistrationPlate,
                     TicketTakeover = p.TicketTakeover,
                     TicketExpiration = p.TicketExpiration,
                     ParkingId = p.ParkingId
                })
                .ToListAsync();

            return Ok(parkingEntries);
        }

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<List<ParkingEntryDto>>> GetAllParkingEntries()
        {
            var parkingEntries = await _dataContext.ParkingEntries
                .Select(p => new ParkingEntryDto
                {
                    Id = p.Id,
                    RegistrationPlate = p.RegistrationPlate,
                    TicketTakeover = p.TicketTakeover,
                    TicketExpiration = p.TicketExpiration,
                    ParkingId = p.ParkingId
                })
                .ToListAsync();

            return Ok(parkingEntries);
        }

        [HttpGet("{id}")]
        //[Authorize]
        public async Task<ActionResult<ParkingEntryDto>> GetParkingEntry(int id)
        {
            var parkingEntry = await _dataContext.ParkingEntries
                .Select(p => new ParkingEntryDto
                {
                    Id = p.Id,
                    RegistrationPlate = p.RegistrationPlate,
                    TicketTakeover = p.TicketTakeover,
                    TicketExpiration = p.TicketExpiration,
                    ParkingId = p.ParkingId
                })
                .FirstOrDefaultAsync();

            return Ok(parkingEntry);
        }

        [HttpDelete]
        //[Authorize]
        public async Task<ActionResult<List<ParkingEntryDto>>> DeleteParkingEntry(int id)
        {
            var dbParkingEntry = await _dataContext.ParkingEntries.FindAsync(id);

            if (dbParkingEntry == null)
                return NotFound("Parking entry not found.");

            _dataContext.ParkingEntries.Remove(dbParkingEntry);
            await _dataContext.SaveChangesAsync();

            var parkingEntries = await _dataContext.ParkingEntries
                .Select(p => new ParkingEntryDto
                {
                    Id = p.Id,
                    RegistrationPlate = p.RegistrationPlate,
                    TicketTakeover = p.TicketTakeover,
                    TicketExpiration = p.TicketExpiration,
                    ParkingId = p.ParkingId
                })
                .ToListAsync();

            return Ok(parkingEntries);
        }

        [HttpPut("{parkingEntryId}/leaves")]
        //[Authorize]
        public async Task<ActionResult<List<ParkingEntryDto>>> AddParkingLeave([FromRoute] int parkingEntryId, [FromBody] CreateParkingLeaveDto createParkingLeaveDto)
        {

            var entry = await _dataContext.ParkingEntries.FindAsync(parkingEntryId);

            if (entry == null)
                return NotFound($"Entry with parkingEntryId {parkingEntryId} not found.");

            var parking = await _dataContext.Parkings.FindAsync(entry.ParkingId);

            if (parking == null)
                return NotFound($"Parking with parkingId {entry.ParkingId} not found.");

            var price = parking.PricePerHour;
            var ticketTakeover = entry.TicketTakeover;
            var ticketExpiration = createParkingLeaveDto.TicketExpiration;

            if (ticketTakeover == null || ticketExpiration == null)
            {
                return BadRequest("TicketTakeover and TicketExpiration must have valid values.");
            }

            if (ticketExpiration <= ticketTakeover)
            {
                return BadRequest("TicketExpiration must be later than TicketTakeover.");
            }

            var duration = ticketExpiration.Value - ticketTakeover.Value;

            var totalHours = (decimal)Math.Ceiling(duration.TotalHours);

            var totalPrice = (int)(totalHours * price);


            entry.TicketExpiration = ticketExpiration;

            var payment = new ParkingPayments
            {
                Amount = totalPrice,
                ParkingEntryId = parkingEntryId
            };

            _dataContext.ParkingPayments.Add(payment);
            await _dataContext.SaveChangesAsync();

            var parkingLeaves = await _dataContext.ParkingEntries
                .Select(p => new ParkingEntryDto
                {
                    Id = p.Id,
                    RegistrationPlate = p.RegistrationPlate,
                    TicketTakeover = p.TicketTakeover,
                    TicketExpiration = p.TicketExpiration,
                    ParkingId = p.ParkingId
                })
                .ToListAsync();

            return Ok(parkingLeaves);
        }

        [HttpGet("{ParkingId}/entries")]
        public async Task<ActionResult<List<ParkingEntryDto>>> GetParkingEntriesForParkingId([FromRoute] int ParkingId)
        {
            var parkingEntries = await _dataContext.ParkingEntries
                .Where(p => p.ParkingId == ParkingId)
                .Select(p => new ParkingEntryDto
                {
                    Id = p.Id,
                    RegistrationPlate = p.RegistrationPlate,
                    TicketTakeover = p.TicketTakeover,
                    TicketExpiration = p.TicketExpiration,
                    ParkingId = p.ParkingId
                })
                .ToListAsync();

            return Ok(parkingEntries);
        }

        [HttpGet("{parkingId}/payments")]
        public async Task<ActionResult<List<ParkingPayments>>> GetPaymentsByParkingId(int parkingId)
        {
            var payments = await _dataContext.ParkingPayments
                .Where(payment => _dataContext.ParkingEntries
                    .Any(entry => entry.Id == payment.ParkingEntryId && entry.ParkingId == parkingId))
                .Select(payment => new ParkingPayments
                {
                    Id = payment.Id,
                    Amount = payment.Amount,
                    ParkingEntryId = payment.ParkingEntryId,
                    Currency = payment.Currency
                })
                .ToListAsync();

            if (!payments.Any())
            {
                return NotFound($"No payments found for Parking ID {parkingId}.");
            }

            return Ok(payments);
        }

    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingSystem.Data;
using ParkingSystem.DTOs;
using ParkingSystem.Entities;
using ParkingSystem.Services;

namespace ParkingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingEntriesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IParkingService _parkingService;
        private readonly IParkingEntryService _parkingEntryService;
        public List<int> Items { get; set; }
        public ParkingEntriesController(IEmployeeService EmployeeSerivce, IParkingService ParkingService, IParkingEntryService ParkingEntryService)
        {
            _employeeService = EmployeeSerivce;
            _parkingService = ParkingService;
            _parkingEntryService = ParkingEntryService;
        }

        [HttpPost("{ParkingId}/entries")]
        [Authorize]
        public async Task<ActionResult<List<ParkingEntryDto>>> AddParkingEntry([FromRoute] int ParkingId, [FromBody] CreateParkingEntryDto createParkingEntryDto)
        {
            var parking = await _parkingService.GetParkingAsync(ParkingId);

            var parkingEntries = await _parkingEntryService.CreateParkingEntryAsync(createParkingEntryDto, ParkingId);

            return Ok(parkingEntries);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<ParkingEntryDto>>> GetAllParkingEntries()
        {
            try
            {
                var parkingEntries = await _parkingEntryService.GetAllParkingEntriesAsync();
                return Ok(parkingEntries);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ParkingEntryDto>> GetParkingEntry(int id)
        {

            try
            {
                var parkingEntry = await _parkingEntryService.GetParkingEntryAsync(id);
                return Ok(parkingEntry);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<List<ParkingEntryDto>>> DeleteParkingEntry(int id)
        {
            var parkingEntries = await _parkingEntryService.DeleteParkingLeave(id);

            return Ok(parkingEntries);
        }

        [HttpPut("{parkingEntryId}/leaves")]
        [Authorize]
        public async Task<ActionResult<List<ParkingEntryDto>>> AddOrUpdateParkingLeave([FromRoute] int parkingEntryId, [FromBody] CreateParkingLeaveDto createParkingLeaveDto)
        {
            try
            {
                var updatedEntries = await _parkingEntryService.UpdateParkingLeaveAsync(createParkingLeaveDto, parkingEntryId);

                return Ok(updatedEntries);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An error occurred while adding or updating the parking leave.");
            }
        }

        [HttpGet("{ParkingId}/entries")]
        [Authorize]
        public async Task<ActionResult<List<ParkingEntryDto>>> GetParkingEntriesByParkingId([FromRoute] int ParkingId)
        {
            var parkingEntries = await _parkingEntryService.GetParkingEntriesByParkingId(ParkingId);

            return Ok(parkingEntries);
        }

        [HttpGet("{parkingId}/payments")]
        [Authorize]
        public async Task<ActionResult<List<ParkingPayments>>> GetPaymentsByParkingId(int parkingId)
        {
            var payments = await _parkingEntryService.GetParkingPaymentsByParkingId(parkingId);

            if (!payments.Any())
            {
                return NotFound($"No payments found for Parking ID {parkingId}.");
            }

            return Ok(payments);
        }
    }
}
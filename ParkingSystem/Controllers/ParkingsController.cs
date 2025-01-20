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
    public class ParkingsController : ControllerBase
    {
        private readonly IParkingService _parkingService;
        public List<int> Items { get; set; }
        public ParkingsController(IParkingService ParkingService)
        {
            _parkingService = ParkingService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<List<ParkingDto>>> AddParking(CreateParkingDto createParkingDto)
        {
            var parkings = await _parkingService.CreateParkingAsync(createParkingDto);

            return Ok(parkings);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<List<ParkingDto>>> UpdateParking(Parking updatedParking)
        {
            var parkings = await _parkingService.UpdateParkingAsync(updatedParking);

            return Ok(parkings);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<ParkingDto>>> GetAllParkings()
        {
            try
            {
                var parkings = await _parkingService.GetAllParkingsAsync();
                return Ok(parkings);
            }
            catch (KeyNotFoundException ex)
            {
                // Return a 404 Not Found with the exception message
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle other unexpected errors
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ParkingDto>> GetParking(int id)
        {
            try
            {
                var parking = await _parkingService.GetParkingAsync(id);
                return Ok(parking);
            }
            catch (KeyNotFoundException ex)
            {
                // Return a 404 Not Found with the exception message
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle other unexpected errors
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<List<ParkingDto>>> DeleteParking(int id)
        {
            var parkings = await _parkingService.DeleteParkingAsync(id);

            return Ok(parkings);
        }
    }
}
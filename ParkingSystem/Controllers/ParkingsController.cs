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
            try
            {
                var parking = await _parkingService.CreateParkingAsync(createParkingDto);

                return Ok(parking);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ParkingDto>> UpdateParking(Parking updatedParking)
        {
            try
            {
                var parking = await _parkingService.UpdateParkingAsync(updatedParking);

                return Ok(parking);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<ParkingDto>>> GetAllParkings()
        {
            var parkings = await _parkingService.GetAllParkingsAsync();
            return Ok(parkings);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ParkingDto>> GetParking(int id)
        {
            var parking = await _parkingService.GetParkingAsync(id);
            return Ok(parking);
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<List<ParkingDto>>> DeleteParking(int id)
        {
            var parking = await _parkingService.DeleteParkingAsync(id);
            return Ok(parking);
        }
    }
}
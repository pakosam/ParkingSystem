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
        private readonly DataContext _dataContext;
        private readonly IParkingService _parkingService;
        public List<int> Items { get; set; }
        public ParkingsController(DataContext dataContext, IParkingService ParkingService)
        {
            _dataContext = dataContext;
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
            var parkings = await _dataContext.Parkings
                .Select(p => new ParkingDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    NumberOfPlaces = p.NumberOfPlaces,
                    OpeningTime = p.OpeningTime,
                    ClosingTime = p.ClosingTime,
                    PricePerHour = (int)p.PricePerHour
                })
                .ToListAsync();

            return Ok(parkings);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ParkingDto>> GetParking(int id)
        {
            var parking = await _dataContext.Parkings
                .Where(p => p.Id == id)
                .Select(p => new ParkingDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    NumberOfPlaces = p.NumberOfPlaces,
                    OpeningTime = p.OpeningTime,
                    ClosingTime = p.ClosingTime,
                    PricePerHour = (int)p.PricePerHour
                })
                .FirstOrDefaultAsync();

            return Ok(parking);
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
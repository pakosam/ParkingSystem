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
    public class ParkingsController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public List<int> Items { get; set; }
        public ParkingsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<List<Parking>>> AddParking(CreateParkingDto createParkingDto)
        {
            var parking = new Parking
            {
                Name = createParkingDto.Name,
                NumberOfPlaces = createParkingDto.NumberOfPlaces,
                OpeningTime = createParkingDto.OpeningTime,
                ClosingTime = createParkingDto.ClosingTime,
                PricePerHour = createParkingDto.PricePerHour
            };

            _dataContext.Parkings.Add(parking);
            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.Parkings.ToListAsync());
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<List<Parking>>> UpdateParking(Parking updatedParking)
        {
            var dbParking = await _dataContext.Parkings.FindAsync(updatedParking.Id);

            if (dbParking == null)
                return NotFound("Parking not found.");

            dbParking.Name = updatedParking.Name;
            dbParking.NumberOfPlaces = updatedParking.NumberOfPlaces;
            dbParking.OpeningTime = updatedParking.OpeningTime;
            dbParking.ClosingTime = updatedParking.ClosingTime;
            dbParking.PricePerHour = updatedParking.PricePerHour;

            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.Parkings.ToListAsync());
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Parking>>> GetAllParkings()
        {
            var parkings = await _dataContext.Parkings.ToListAsync();

            return Ok(parkings);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Parking>> GetParking(int id)
        {
            var parking = await _dataContext.Parkings.FindAsync(id);

            if (parking == null)
                return NotFound("Parking not found.");

            return Ok(parking);
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<List<Parking>>> DeleteParking(int id)
        {
            var dbParking = await _dataContext.Parkings.FindAsync(id);

            if (dbParking == null)
                return NotFound("Parking not found.");

            var employeesWithParking = await _dataContext.Employees
                .Where(e => e.ParkingId == id)
                .ToListAsync();

            foreach (var employee in employeesWithParking)
            {
                employee.ParkingId = null;
            }

            _dataContext.Parkings.Remove(dbParking);
            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.Parkings.ToListAsync());
        }
    }
}
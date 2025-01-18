using Microsoft.EntityFrameworkCore;
using ParkingSystem.Data;
using ParkingSystem.DTOs;
using ParkingSystem.Entities;

namespace ParkingSystem.Repositories
{
    public class ParkingRepository : IParkingRepository
    {
        private readonly DataContext _dataContext;

        public ParkingRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task CreateParkingAsync(Parking parking)
        {
            _dataContext.Parkings.Add(parking);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<ParkingDto>> GetAllParkingsAsync()
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
            return parkings;
        }
    }
}

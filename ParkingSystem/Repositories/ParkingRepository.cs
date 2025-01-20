using Microsoft.AspNetCore.Mvc;
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

        public async Task<List<Parking>> GetAllParkingsAsync()
        {
            var parkings = await _dataContext.Parkings.ToListAsync();
            return parkings;
        }

        public async Task<Parking> GetParkingAsync(int id)
        {
            var parking = await _dataContext.Parkings
                .FirstOrDefaultAsync(p => p.Id == id);
            return parking;
        }

        public async Task UpdateParkingAsync(Parking updatedParking)
        {
            _dataContext.Parkings.Update(updatedParking);
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteParkingAsync(Parking parking)
        {
            _dataContext.Parkings.Remove(parking);
            await _dataContext.SaveChangesAsync();
        }
    }
}

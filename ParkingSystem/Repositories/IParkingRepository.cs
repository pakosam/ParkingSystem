using ParkingSystem.DTOs;
using ParkingSystem.Entities;

namespace ParkingSystem.Repositories
{
    public interface IParkingRepository
    {
        Task CreateParkingAsync(Parking parking);
        Task UpdateParkingAsync(Parking updatedParking);
        Task<List<ParkingDto>> GetAllParkingsAsync();
    }
}

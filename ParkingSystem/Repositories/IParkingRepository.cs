using ParkingSystem.DTOs;
using ParkingSystem.Entities;

namespace ParkingSystem.Repositories
{
    public interface IParkingRepository
    {
        Task CreateParkingAsync(Parking parking);
        Task<List<ParkingDto>> GetAllParkingsAsync();
    }
}

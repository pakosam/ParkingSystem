using ParkingSystem.DTOs;
using ParkingSystem.Entities;

namespace ParkingSystem.Services
{
    public interface IParkingService
    {
        Task<List<ParkingDto>> CreateParkingAsync(CreateParkingDto parking);
        Task<List<ParkingDto>> UpdateParkingAsync(Parking updatedParking);
        Task<List<ParkingDto>> GetAllParkingsAsync();
        Task<ParkingDto> GetParkingAsync(int id);
        Task<List<ParkingDto>> DeleteParkingAsync(int id);
    }
}

using ParkingSystem.DTOs;

namespace ParkingSystem.Repositories
{
    public interface IEmployeeRepository
    {
        Task RemoveParkingIdFromEmployeesAsync(int parkingId);
    }
}

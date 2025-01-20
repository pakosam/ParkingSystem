using ParkingSystem.DTOs;
using ParkingSystem.Entities;

namespace ParkingSystem.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetEmployeesByParkingIdAsync(int parkingId);
        Task UpdateEmployeesParkingIdAsync(List<Employee> employees, int parkingId);
    }
}

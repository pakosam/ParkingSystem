using ParkingSystem.DTOs;
using ParkingSystem.Entities;

namespace ParkingSystem.Repositories
{
    public interface IEmployeeRepository
    {

        Task CreateEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee updatedEmployee);
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeAsync(int id);
        Task DeleteEmployeeAsync(Employee employee);




        Task<List<Employee>> GetEmployeesByParkingIdAsync(int parkingId);
        Task UpdateEmployeesParkingIdAsync(List<Employee> employees, int parkingId);
    }
}

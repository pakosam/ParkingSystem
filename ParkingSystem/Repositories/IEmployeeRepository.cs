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
        Task<List<int>> GetEmployeesIdByParkingIdAsync(int parkingId);
        Task UpdateEmployeesParkingIdAsync(List<int> employeeIds);
        Task<bool> UsernameExistsAsync(string username);
        Task<Employee> GetEmployeeByUsernameAsync(string username);
        Task<List<Employee>> GetEmployeesByIdsAsync(List<int> employeeIds);
    }
}

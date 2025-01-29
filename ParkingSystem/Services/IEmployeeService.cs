using ParkingSystem.DTOs;
using ParkingSystem.Entities;

namespace ParkingSystem.Services
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> CreateEmployeeAsync(CreateEmployeeDto employee, int parkingId);
        Task<List<EmployeeDto>> UpdateEmployeeAsync(UpdateEmployeeDto updatedEmployee, int parkingId);
        Task<List<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto> GetEmployeeAsync(int id);
        Task<List<EmployeeDto>> DeleteEmployeeAsync(int id);
        Task<List<int>> GetEmployeesIdByParkingIdAsync(int id);
        Task UpdateEmployeesParkingIdAsync(List<int> employeeIds);
    }
}

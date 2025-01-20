using ParkingSystem.Data;
using ParkingSystem.DTOs;
using ParkingSystem.Entities;
using ParkingSystem.Repositories;

namespace ParkingSystem.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
    }
}

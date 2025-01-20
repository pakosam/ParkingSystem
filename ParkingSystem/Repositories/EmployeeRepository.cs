using Microsoft.EntityFrameworkCore;
using ParkingSystem.Data;
using ParkingSystem.DTOs;
using ParkingSystem.Entities;

namespace ParkingSystem.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _dataContext;

        public EmployeeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Employee>> GetEmployeesByParkingIdAsync (int parkingId)
        {
            var employeesByParkingId = await _dataContext.Employees 
                .Where(e => e.ParkingId == parkingId) // GetEMployyeesByParkingIdAsync
                .ToListAsync();

            return employeesByParkingId;
        }

        public async Task UpdateEmployeesParkingIdAsync(List<Employee> employees, int parkingId)
        {
            foreach (var employee in employees) // ForPetlja ide u service
            {
                employee.ParkingId = null; // Ovo ide u UpdateEmployee
            }

            await _dataContext.SaveChangesAsync();
        }
    }
}

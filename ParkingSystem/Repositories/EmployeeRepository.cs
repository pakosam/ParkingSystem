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

        public async Task CreateEmployeeAsync(Employee employee)
        {
            _dataContext.Employees.Add(employee);
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(Employee employee)
        {
            _dataContext.Employees.Remove(employee);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            Console.WriteLine("Fetching all employees...");
            var employees = await _dataContext.Employees.ToListAsync();
            Console.WriteLine($"Found {employees.Count} employees.");
            return employees;
        }

        public async Task<Employee> GetEmployeeAsync(int id)
        {
            var employee = await _dataContext.Employees
                .FirstOrDefaultAsync(e => e.Id == id);
            return employee;
        }

        public async Task UpdateEmployeeAsync(Employee updatedEmployee)
        {
            _dataContext.Employees.Update(updatedEmployee);
            await _dataContext.SaveChangesAsync();
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

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
            var employees = await _dataContext.Employees.ToListAsync();
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
                .Where(e => e.ParkingId == parkingId) 
                .ToListAsync();

            return employeesByParkingId;
        }

        public async Task UpdateEmployeesParkingIdAsync(List<Employee> employees, int parkingId)
        {
            foreach (var employee in employees) 
            {
                employee.ParkingId = null; 
            }

            await _dataContext.SaveChangesAsync();
        }
    }
}

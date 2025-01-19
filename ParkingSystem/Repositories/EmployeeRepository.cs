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

        public async Task RemoveParkingIdFromEmployeesAsync(int parkingId)
        {
            var employeesWithParking = await _dataContext.Employees
                .Where(e => e.ParkingId == parkingId)
                .ToListAsync();

            foreach (var employee in employeesWithParking)
            {
                employee.ParkingId = null;
            }

            await _dataContext.SaveChangesAsync();
        }

    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingSystem.Data;
using ParkingSystem.DTOs;
using ParkingSystem.Entities;

namespace ParkingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public List <int> Items { get; set; }
        public EmployeesController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        public async Task<ActionResult<List<Employee>>> AddEmployee(CreateEmployeeDto createEmployeeDto, int parkingId)
        {
            var parking = await _dataContext.Parkings.FindAsync(parkingId);

            if (parking == null)
            {
                return NotFound($"Parking with ID {parkingId} not found.");
            }
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(createEmployeeDto.Password);

            var employee = new Employee
            {
                Name = createEmployeeDto.Name,
                Surname = createEmployeeDto.Surname,
                BirthDate = createEmployeeDto.BirthDate,
                Username = createEmployeeDto.Username,
                Password = hashedPassword,
                ParkingId = parkingId
            };

            _dataContext.Employees.Add(employee);
            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.Employees.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<Employee>>> UpdateEmployee(Employee updatedEmployee, int parkingId)
        {
            var parking = await _dataContext.Parkings.FindAsync(parkingId);

            if (parking == null)
            {
                return NotFound($"Parking with ID {parkingId} not found.");
            }

            var dbEmployee = await _dataContext.Employees.FindAsync(updatedEmployee.Id);

            if (dbEmployee == null)
                return NotFound("Employee not found.");
            
            dbEmployee.Name = updatedEmployee.Name;
            dbEmployee.Surname = updatedEmployee.Surname;
            dbEmployee.BirthDate = updatedEmployee.BirthDate;
            dbEmployee.Username = updatedEmployee.Username;
            dbEmployee.Password = updatedEmployee.Password;
            dbEmployee.ParkingId = parkingId;

            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.Employees.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetAllEmployees()
        {
            var employees = await _dataContext.Employees.ToListAsync();

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _dataContext.Employees.FindAsync(id);

            if (employee == null)
                return NotFound("Employee not found.");

            await _dataContext.SaveChangesAsync();

            return Ok(employee);
        }

        [HttpDelete]
        public async Task<ActionResult<List<Employee>>> DeleteEmployee(int id)
        {
            var dbEmployee = await _dataContext.Employees.FindAsync(id);

            if (dbEmployee == null)
                return NotFound("Employee not found.");

            _dataContext.Employees.Remove(dbEmployee);
            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.Employees.ToListAsync());
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.MicrosoftExtensions;
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
        [Authorize]
        public async Task<ActionResult<List<EmployeeDto>>> AddEmployee([FromBody] CreateEmployeeDto createEmployeeDto, [FromQuery] int parkingId)
        {
            var parking = await _dataContext.Parkings.FindAsync(parkingId);

            if (parking == null)
            {
                return NotFound($"Parking with ID {parkingId} not found.");
            }

            var today = DateOnly.FromDateTime(DateTime.Today);

            if (createEmployeeDto.BirthDate == DateOnly.MinValue)
            {
                return BadRequest("Birth date is required");
            }

            var age = today.Year - createEmployeeDto.BirthDate.Year;
            if (createEmployeeDto.BirthDate > today.AddYears(-age))
            {
                age--;
            }

            if (age > 70)
            {
                return BadRequest("Employee cannot be older than 70 years.");
            }

            if (createEmployeeDto.Name.Length < 3 ||
                createEmployeeDto.Surname.Length < 3 ||
                createEmployeeDto.Username.Length < 4 ||
                createEmployeeDto.Password.Length < 10)
            {
                return BadRequest("Data is not filled properly");
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

            var employees = await _dataContext.Employees
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FullName = e.Name + " " + e.Surname,
                    BirthDate = e.BirthDate,
                    ParkingId = e.ParkingId ?? 0
                })
                .ToListAsync();

            return Ok(employees);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<List<EmployeeDto>>> UpdateEmployee(UpdateEmployeeDto updatedEmployee, int parkingId)
        {
            var parking = await _dataContext.Parkings.FindAsync(parkingId);

            if (parking == null)
            {
                return NotFound($"Parking with ID {parkingId} not found.");
            }

            var dbEmployee = await _dataContext.Employees.FindAsync(updatedEmployee.Id);

            if (dbEmployee == null)
                return NotFound("Employee not found.");

            var today = DateOnly.FromDateTime(DateTime.Today);

            if (updatedEmployee.BirthDate == DateOnly.MinValue)
            {
                return BadRequest("Birth date is required");
            }

            var age = today.Year - updatedEmployee.BirthDate.Year;
            if (updatedEmployee.BirthDate > today.AddYears(-age))
            {
                age--;
            }

            if (age > 70)
            {
                return BadRequest("Employee cannot be older than 70 years.");
            }

            dbEmployee.Name = updatedEmployee.Name;
            dbEmployee.Surname = updatedEmployee.Surname;
            dbEmployee.BirthDate = updatedEmployee.BirthDate;
            dbEmployee.ParkingId = parkingId;

            if (dbEmployee.Name.Length < 3 ||
                dbEmployee.Surname.Length <= 3)
            {
                return BadRequest("Data is not filled properly");
            }

            await _dataContext.SaveChangesAsync();

            var employees = await _dataContext.Employees
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FullName = e.Name + " " + e.Surname,
                    BirthDate = e.BirthDate,
                    ParkingId = e.ParkingId ?? 0
                })
                .ToListAsync();

            return Ok(employees);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<EmployeeDto>>> GetAllEmployees()
        {
            var employees = await _dataContext.Employees
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FullName = e.Name + " " + e.Surname,
                    BirthDate = e.BirthDate,
                    ParkingId = e.ParkingId ?? 0
                })
                .ToListAsync();

            return Ok(employees);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            var employee = await _dataContext.Employees
                .Where(p => p.Id == id)
                .Select(e => new UpdateEmployeeDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Surname = e.Surname,
                    BirthDate = e.BirthDate,
                    ParkingId = e.ParkingId ?? 0
                })
                .FirstOrDefaultAsync();

            if (employee == null)
                return NotFound("Employee not found.");

            return Ok(employee);
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<List<EmployeeDto>>> DeleteEmployee(int id)
        {
            var dbEmployee = await _dataContext.Employees.FindAsync(id);

            if (dbEmployee == null)
                return NotFound("Employee not found.");

            _dataContext.Employees.Remove(dbEmployee);
            await _dataContext.SaveChangesAsync();

            var employees = await _dataContext.Employees
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FullName = e.Name + " " + e.Surname,
                    BirthDate = e.BirthDate,
                    ParkingId = e.ParkingId ?? 0
                })
                .ToListAsync();

            return Ok(employees);
        }
    }
}

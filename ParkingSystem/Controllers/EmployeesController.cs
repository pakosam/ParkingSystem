using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.MicrosoftExtensions;
using ParkingSystem.Data;
using ParkingSystem.DTOs;
using ParkingSystem.Entities;
using ParkingSystem.Services;

namespace ParkingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IParkingService _parkingService;

        public List <int> Items { get; set; }
        public EmployeesController(IEmployeeService EmployeeSerivce, IParkingService ParkingService)
        {
            _employeeService = EmployeeSerivce;
            _parkingService = ParkingService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<List<EmployeeDto>>> AddEmployee([FromBody] CreateEmployeeDto createEmployeeDto, [FromQuery] int parkingId)
        {
            try
            {
                var parking = await _parkingService.GetParkingAsync(parkingId);

                var employees = await _employeeService.CreateEmployeeAsync(createEmployeeDto, parkingId);

                return Ok(employees);
            }
            catch (ArgumentException ex) 
            {
                return BadRequest(new { message = ex.Message }); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {error = ex.Message });
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<List<EmployeeDto>>> UpdateEmployee(UpdateEmployeeDto updatedEmployee, int parkingId)
        {
            try
            {
                var parking = await _parkingService.GetParkingAsync(parkingId);

                var employees = await _employeeService.UpdateEmployeeAsync(updatedEmployee, parkingId);

                return Ok(employees);
            }
            catch (ArgumentException ex) 
            {
                return BadRequest(new { message = ex.Message }); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {error = ex.Message });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<EmployeeDto>>> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();
                return Ok(employees);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeAsync(id);
                return Ok(employee);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<List<EmployeeDto>>> DeleteEmployee(int id)
        {
            var employees = await _employeeService.DeleteEmployeeAsync(id);

            return Ok(employees);
        }
    }
}

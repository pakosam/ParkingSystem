using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ParkingSystem.Data;
using ParkingSystem.DTOs;
using ParkingSystem.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ParkingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;
        public AuthorizationsController(IConfiguration configuration, DataContext dataContext)
        {
            _configuration = configuration;
            _dataContext = dataContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateEmployeeDto registerDto)
        {
            var existingUser = await _dataContext.Employees
                .AnyAsync(e => e.Username == registerDto.Username);

            if (existingUser)
            {
                return BadRequest("Username already exists.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var employee = new Employee
            {
                Username = registerDto.Username,
                Password = hashedPassword,
                Name = registerDto.Name,
                Surname = registerDto.Surname,
                BirthDate = registerDto.BirthDate
            };

            _dataContext.Employees.Add(employee);
            await _dataContext.SaveChangesAsync();

            return Ok("Registration successful.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var employee = await _dataContext.Employees
                .FirstOrDefaultAsync(e => e.Username == loginDto.Username);

            if (employee != null && BCrypt.Net.BCrypt.Verify(loginDto.Password, employee.Password))
            {
                var token = GenerateJwtToken(employee.Username);
                return Ok(new { token });
            }

            return Unauthorized("Invalid username or/and password");
        }

        private string GenerateJwtToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
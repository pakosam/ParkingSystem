using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ParkingSystem.Data;
using ParkingSystem.DTOs;
using ParkingSystem.Entities;
using ParkingSystem.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ParkingSystem.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IConfiguration _configuration;
        public AuthorizationService(IEmployeeRepository employeeRepository, IConfiguration configuration)
        {
            _employeeRepository = employeeRepository;
            _configuration = configuration;
        }
        public async Task RegisterAsync(CreateEmployeeDto registerDto)
        {
            var existingUser = await _employeeRepository.UsernameExistsAsync(registerDto.Username);
            if (existingUser)
            {
                throw new ArgumentException("Username already exists.");
            }
            

            var today = DateOnly.FromDateTime(DateTime.Today);

            if (registerDto.BirthDate == DateOnly.MinValue)
            {
                throw new NotImplementedException("Birth date is required");
            }

            var age = today.Year - registerDto.BirthDate.Year;
            if (registerDto.BirthDate > today.AddYears(-age))
            {
                age--;
            }

            if (age > 70)
            {
                throw new NotImplementedException("Employee cannot be older than 70 years.");
            }

            if (age < 18)
            {
                throw new NotImplementedException("Employee cannot be younger than 18 years.");
            }

            if (registerDto.Name.Length < 3 ||
                registerDto.Surname.Length < 3 ||
                registerDto.Username.Length < 4 ||
                registerDto.Password.Length < 10)
            {
                throw new NotImplementedException("Data is not filled properly");
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

            await _employeeRepository.CreateEmployeeAsync(employee);
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var employee = await _employeeRepository.GetEmployeeByUsernameAsync(loginDto.Username);
            if (employee == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, employee.Password))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            return GenerateJwtToken(employee.Username);
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
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

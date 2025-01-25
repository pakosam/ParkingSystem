using Microsoft.AspNetCore.Mvc;
using ParkingSystem.DTOs;

namespace ParkingSystem.Services
{
    public interface IAuthorizationService
    {
        Task RegisterAsync(CreateEmployeeDto registerDto);
        Task<string> LoginAsync(LoginDto loginDto);
    }
}

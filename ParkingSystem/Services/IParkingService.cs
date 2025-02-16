﻿using ParkingSystem.DTOs;
using ParkingSystem.Entities;

namespace ParkingSystem.Services
{
    public interface IParkingService
    {
        Task<ParkingDto> CreateParkingAsync(CreateParkingDto parking);
        Task<ParkingDto> UpdateParkingAsync(Parking updatedParking);
        Task<List<ParkingDto>> GetAllParkingsAsync();
        Task<ParkingDto> GetParkingAsync(int id);
        Task<ParkingDto> DeleteParkingAsync(int id);
    }
}

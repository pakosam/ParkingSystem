using Microsoft.AspNetCore.Mvc;
using ParkingSystem.DTOs;
using ParkingSystem.Entities;

namespace ParkingSystem.Services
{
    public interface IParkingEntryService
    {
        Task<List<ParkingEntryDto>> CreateParkingEntryAsync(CreateParkingEntryDto parkingEntryDto, int parkingId);
        Task<List<ParkingEntryDto>> UpdateParkingLeaveAsync(CreateParkingLeaveDto parkingLeaveDto, int parkingEntryId);
        Task<List<ParkingEntryDto>> DeleteParkingLeave(int id);
        Task<List<ParkingEntryDto>> GetAllParkingEntriesAsync();
        Task<ParkingEntryDto> GetParkingEntryAsync(int id);
        Task<List<ParkingEntryDto>> GetParkingEntriesByParkingId(int ParkingId);
        Task<List<ParkingPayments>> GetParkingPaymentsByParkingId(int parkingId);
    }
}

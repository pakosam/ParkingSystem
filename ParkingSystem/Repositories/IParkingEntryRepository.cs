using ParkingSystem.DTOs;
using ParkingSystem.Entities;

namespace ParkingSystem.Repositories
{
    public interface IParkingEntryRepository
    {
        Task CreateParkingEntryAsync(ParkingEntry parkingEntry);
        Task UpdateParkingLeaveAsync(ParkingEntry parkingLeave , int parkingEntryId);
        Task DeleteParkingLeave(ParkingEntry parkingLeave);
        Task<List<ParkingEntry>> GetAllParkingEntriesAsync();
        Task<ParkingEntry> GetParkingEntryAsync(int id);
        Task<ParkingEntry> GetActiveParkingEntryByRegistrationPlateAsync(string registrationPlate);
        Task<ParkingEntry> GetMostRecentParkingEntryByRegistrationPlateAsync(string registrationPlate);
        Task<List<ParkingEntry>> GetParkingEntriesByParkingIdAsync(int ParkingId);
        Task<List<ParkingPayments>> GetParkingPaymentsByParkingIdAsync(int parkingId);
        Task AddOrUpdatePaymentAsync(ParkingPayments payment);
        Task<int> GetActiveEntriesCountByParkingIdAsync(int parkingId);
    }
}

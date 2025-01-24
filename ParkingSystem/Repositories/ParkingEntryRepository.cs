using Microsoft.EntityFrameworkCore;
using ParkingSystem.Data;
using ParkingSystem.Entities;

namespace ParkingSystem.Repositories
{
    public class ParkingEntryRepository : IParkingEntryRepository
    {
        private readonly DataContext _dataContext;

        public ParkingEntryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task CreateParkingEntryAsync(ParkingEntry parkingEntry)
        {
            _dataContext.ParkingEntries.Add(parkingEntry);
            await _dataContext.SaveChangesAsync();
        }

        public async Task UpdateParkingLeaveAsync(ParkingEntry parkingLeave, int parkingEntryId)
        {
            var existingEntry = await _dataContext.ParkingEntries
                .FirstOrDefaultAsync(p => p.Id == parkingEntryId);

            if (existingEntry != null)
            {
                existingEntry.TicketExpiration = parkingLeave.TicketExpiration;
            }

            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteParkingLeave(ParkingEntry parkingLeave)
        {
            _dataContext.ParkingEntries.Remove(parkingLeave);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<ParkingEntry>> GetAllParkingEntriesAsync()
        {
            var parkingEntries = await _dataContext.ParkingEntries.ToListAsync();
            return parkingEntries;
        }

        public async Task<ParkingEntry> GetParkingEntryAsync(int id)
        {
            var parkingEntry = await _dataContext.ParkingEntries
                .FirstOrDefaultAsync(p => p.Id == id);
            return parkingEntry;
        }

        public async Task<ParkingEntry> GetActiveParkingEntryByRegistrationPlateAsync(string registrationPlate)
        {
            var activeParkingEntry = await _dataContext.ParkingEntries
                .FirstOrDefaultAsync(entry => entry.RegistrationPlate == registrationPlate && entry.TicketExpiration == null);
            return activeParkingEntry;
        }

        public async Task<ParkingEntry> GetMostRecentParkingEntryByRegistrationPlateAsync(string registrationPlate)
        {
            var recentLeave = await _dataContext.ParkingEntries
                .Where(entry => entry.RegistrationPlate == registrationPlate)
                .OrderByDescending(entry => entry.TicketTakeover)
                .FirstOrDefaultAsync();
            return recentLeave;
        }

        public async Task<List<ParkingEntry>> GetParkingEntriesByParkingIdAsync(int ParkingId)
        {
            var existingEntry = await _dataContext.ParkingEntries
                .Where(p => p.ParkingId == ParkingId)
                .ToListAsync();
            return existingEntry;
        }

        public async Task<List<ParkingPayments>> GetParkingPaymentsByParkingIdAsync(int parkingId)
        {
            var payments = await _dataContext.ParkingPayments
                 .Join(
                     _dataContext.ParkingEntries,
                     payment => payment.ParkingEntryId,
                     entry => entry.Id,
                     (payment, entry) => new { Payment = payment, Entry = entry }
                 )
                 .Where(joined => joined.Entry.ParkingId == parkingId) 
                 .Select(joined => joined.Payment)
                 .ToListAsync();

            return payments;
        }

        public async Task AddOrUpdatePaymentAsync(ParkingPayments payment)
        {
            var existingPayment = await _dataContext.ParkingPayments
                .FirstOrDefaultAsync(p => p.ParkingEntryId == payment.ParkingEntryId);

            if (existingPayment != null)
            {
                existingPayment.Amount = payment.Amount;
            }
            else
            {
                _dataContext.ParkingPayments.Add(payment);
            }

            await _dataContext.SaveChangesAsync();
        }
    }
}

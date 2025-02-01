using ParkingSystem.Data;
using ParkingSystem.DTOs;
using ParkingSystem.Entities;
using ParkingSystem.Repositories;
using System.Linq;

namespace ParkingSystem.Services
{
    public class ParkingEntryService : IParkingEntryService
    {
        private readonly IParkingEntryRepository _parkingEntryRepository;
        private readonly IParkingRepository _parkingRepository;


        public ParkingEntryService(IParkingEntryRepository parkingEntryRepository, IParkingRepository parkingRepository)
        {
            _parkingEntryRepository = parkingEntryRepository;
            _parkingRepository = parkingRepository;
        }

        public async Task<List<ParkingEntryDto>> CreateParkingEntryAsync(CreateParkingEntryDto parkingEntryDto, int parkingId)
        {
            var parking = await _parkingRepository.GetParkingAsync(parkingId);

            if (parking == null)
                throw new ArgumentException($"Parking with ID {parkingId} does not exist.");

            // Count active entries (entries without TicketExpiration)
            var activeEntriesCount = await _parkingEntryRepository.GetActiveEntriesCountByParkingIdAsync(parkingId);
            var remainingPlaces = parking.NumberOfPlaces - activeEntriesCount;

            if (remainingPlaces <= 0)
                throw new ArgumentException("All parking places are full.");

            if (parkingEntryDto.TicketTakeover.HasValue)
            {
                var ticketTakeoverTime = parkingEntryDto.TicketTakeover.Value.TimeOfDay;
                if (ticketTakeoverTime < parking.OpeningTime || ticketTakeoverTime > parking.ClosingTime)
                {
                    throw new ArgumentException($"TicketTakeover must be between the parking's opening time ({parking.OpeningTime}) and closing time ({parking.ClosingTime}).");
                }
            }

            var activeEntry = await _parkingEntryRepository.GetActiveParkingEntryByRegistrationPlateAsync(parkingEntryDto.RegistrationPlate);
            if (activeEntry != null)
            {
                throw new ArgumentException($"A parking entry already exists for the registration plate '{parkingEntryDto.RegistrationPlate}', and the car has not left yet.");
            }

            var previousEntry = await _parkingEntryRepository.GetMostRecentParkingEntryByRegistrationPlateAsync(parkingEntryDto.RegistrationPlate);
            if (previousEntry != null && previousEntry.TicketExpiration.HasValue)
            {
                if (parkingEntryDto.TicketTakeover <= previousEntry.TicketExpiration)
                {
                    throw new ArgumentException($"The new entry's TicketTakeover must be later than the previous leave time ({previousEntry.TicketExpiration}).");
                }
            }

            var parkingEntry = new ParkingEntry
            {
                RegistrationPlate = parkingEntryDto.RegistrationPlate,
                TicketTakeover = parkingEntryDto.TicketTakeover,
                ParkingId = parkingId
            };

            await _parkingEntryRepository.CreateParkingEntryAsync(parkingEntry);

            var parkingEntries = await _parkingEntryRepository.GetAllParkingEntriesAsync();

            var parkingEntryDtos = parkingEntries
                .Select(p => new ParkingEntryDto
                {
                    Id = p.Id,
                    RegistrationPlate = p.RegistrationPlate,
                    TicketTakeover = p.TicketTakeover,
                    TicketExpiration = p.TicketExpiration,
                    ParkingId = p.ParkingId
                })
                .ToList();

            return parkingEntryDtos;
        }

        public async Task<List<ParkingEntryDto>> UpdateParkingLeaveAsync(CreateParkingLeaveDto parkingLeaveDto, int parkingEntryId)
        {
            var dbEntry = await _parkingEntryRepository.GetParkingEntryAsync(parkingEntryId);   

            if (dbEntry == null)
                throw new ArgumentException($"Entry with ID {parkingEntryId} does not exist."); 

            var parking = await _parkingRepository.GetParkingAsync(dbEntry.ParkingId);             

            if (parking == null)
                throw new ArgumentException($"Parking with ID {dbEntry.ParkingId} not found."); 

            var price = parking.PricePerHour;
            var ticketTakeover = dbEntry.TicketTakeover;
            var ticketExpiration = parkingLeaveDto.TicketExpiration;

            if (ticketTakeover == null || ticketExpiration == null) 
            {
                throw new ArgumentException("TicketTakeover and TicketExpiration must have valid values.");
            }

            if (ticketExpiration <= ticketTakeover) 
            {
                throw new ArgumentException("TicketExpiration must be later than TicketTakeover.");
            }

            if (ticketExpiration.HasValue)
            {
                var ticketExpirationTime = ticketExpiration.Value.TimeOfDay;
                if (ticketExpirationTime < parking.OpeningTime || ticketExpirationTime > parking.ClosingTime)
                {
                    throw new ArgumentException($"TicketExpiration must be between the parking's opening time ({parking.OpeningTime}) and closing time ({parking.ClosingTime}).");
                }
            }

            var duration = ticketExpiration.Value - ticketTakeover.Value; 
            var totalHours = (decimal)Math.Ceiling(duration.TotalHours);  
            var totalPrice = (int)(totalHours * price);                   

            var payment = new ParkingPayments
            {
                Amount = totalPrice,
                ParkingEntryId = parkingEntryId
            };
            await _parkingEntryRepository.AddOrUpdatePaymentAsync(payment);

            dbEntry.TicketExpiration = ticketExpiration;
            await _parkingEntryRepository.UpdateParkingLeaveAsync(dbEntry, parkingEntryId);

            var parkingEntries = await _parkingEntryRepository.GetAllParkingEntriesAsync();

            var parkingEntryDtos = parkingEntries
                .Select(entry => new ParkingEntryDto
                {
                    Id = entry.Id,
                    RegistrationPlate = entry.RegistrationPlate,
                    TicketTakeover = entry.TicketTakeover,
                    TicketExpiration = entry.TicketExpiration,
                    ParkingId = entry.ParkingId
                })
                .ToList();

            return parkingEntryDtos;
        }

        public async Task<List<ParkingEntryDto>> DeleteParkingLeave(int id)
        {
            var parkingEntry = await _parkingEntryRepository.GetParkingEntryAsync(id);

            if (parkingEntry == null)
                throw new ArgumentException($"Entry/leave with ID {id} does not exist.");

            await _parkingEntryRepository.DeleteParkingLeave(parkingEntry);

            var parkingEntries = await _parkingEntryRepository.GetAllParkingEntriesAsync();

            var parkingEntryDtos = parkingEntries
                .Select(p => new ParkingEntryDto
                {
                    Id = p.Id,
                    RegistrationPlate = p.RegistrationPlate,
                    TicketTakeover = p.TicketTakeover,
                    TicketExpiration = p.TicketExpiration,
                    ParkingId = p.ParkingId
                })
                .ToList();

            return parkingEntryDtos;
        }

        public async Task<List<ParkingEntryDto>> GetAllParkingEntriesAsync()
        {
            var parkingEntries = await _parkingEntryRepository.GetAllParkingEntriesAsync();

            if (parkingEntries == null)
            {
                throw new KeyNotFoundException($"There are no existing parking entries.");
            }

            var parkingEntryDtos = parkingEntries
                 .Select(p => new ParkingEntryDto
                 {
                     Id = p.Id,
                     RegistrationPlate = p.RegistrationPlate,
                     TicketTakeover = p.TicketTakeover,
                     TicketExpiration = p.TicketExpiration,
                     ParkingId = p.ParkingId
                 })
                 .ToList();

            return parkingEntryDtos;
        }

        public async Task<ParkingEntryDto> GetParkingEntryAsync(int id)
        {
            var parkingEntry = await _parkingEntryRepository.GetParkingEntryAsync(id);

            if (parkingEntry == null)
            {
                throw new KeyNotFoundException($"Parking entry with ID {id} does not exist.");
            }

            return new ParkingEntryDto
            {
                Id = parkingEntry.Id,
                RegistrationPlate = parkingEntry.RegistrationPlate,
                TicketTakeover = parkingEntry.TicketTakeover,
                TicketExpiration = parkingEntry.TicketExpiration,
                ParkingId = parkingEntry.ParkingId
            };
        }

        public async Task<List<ParkingEntryDto>> GetParkingEntriesByParkingId(int ParkingId)
        {
            var parkingEntriesByParkingId = await _parkingEntryRepository.GetParkingEntriesByParkingIdAsync(ParkingId);

            if (parkingEntriesByParkingId == null)
            {
                throw new KeyNotFoundException($"There are no existing parking entries for parking with ID {ParkingId}.");
            }

            var parkingEntryDtos = parkingEntriesByParkingId
                .Where(p => p.ParkingId == ParkingId)
                .Select(p => new ParkingEntryDto
                {
                    Id = p.Id,
                    RegistrationPlate = p.RegistrationPlate,
                    TicketTakeover = p.TicketTakeover,
                    TicketExpiration = p.TicketExpiration,
                    ParkingId = p.ParkingId
                })
                .ToList();

            return parkingEntryDtos;
        }

        public async Task<List<ParkingPayments>> GetParkingPaymentsByParkingId(int parkingId)
        {
            var parkingPaymentsByParkingId = await _parkingEntryRepository.GetParkingPaymentsByParkingIdAsync(parkingId);

            if (parkingPaymentsByParkingId == null || !parkingPaymentsByParkingId.Any())
            {
                throw new KeyNotFoundException($"There are no existing parking payments for parking with ID {parkingId}.");
            }

            return parkingPaymentsByParkingId;
        }
    }
}

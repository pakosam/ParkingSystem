using ParkingSystem.Data;
using ParkingSystem.DTOs;
using ParkingSystem.Entities;
using ParkingSystem.Repositories;

namespace ParkingSystem.Services
{
    public class ParkingService : IParkingService
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public ParkingService(IParkingRepository parkingRepository, IEmployeeRepository employeeRepository)
        {
            _parkingRepository = parkingRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<List<ParkingDto>> CreateParkingAsync(CreateParkingDto parkingDto)
        {
            if (parkingDto.Name.Length < 3 || parkingDto.NumberOfPlaces <= 0 || parkingDto.PricePerHour <= 0 || parkingDto.ClosingTime <= parkingDto.OpeningTime)
                throw new ArgumentException("Data is not filled properly");


            var parking = new Parking
            {
                Name = parkingDto.Name,
                NumberOfPlaces = parkingDto.NumberOfPlaces,
                OpeningTime = parkingDto.OpeningTime,
                ClosingTime = parkingDto.ClosingTime,
                PricePerHour = parkingDto.PricePerHour
            };

            await _parkingRepository.CreateParkingAsync(parking);

            var parkings = await _parkingRepository.GetAllParkingsAsync();

            var parkingDtos = parkings
                .Select(p => new ParkingDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    NumberOfPlaces = p.NumberOfPlaces,
                    OpeningTime = p.OpeningTime,
                    ClosingTime = p.ClosingTime,
                    PricePerHour = (int)p.PricePerHour
                })
                .ToList();

            return parkingDtos;
        }

        public async Task<List<ParkingDto>> DeleteParkingAsync(int id)
        {
            var parking = await _parkingRepository.GetParkingAsync(id);

            if (parking == null)
                throw new ArgumentException($"Parking with ID {id} does not exist.");

            var employeesByParkingId = await _employeeRepository.GetEmployeesByParkingIdAsync(id);

            await _employeeRepository.UpdateEmployeesParkingIdAsync(employeesByParkingId, id);
            
            await _parkingRepository.DeleteParkingAsync(parking);

            var parkings = await _parkingRepository.GetAllParkingsAsync();

            var parkingDtos = parkings
                .Select(p => new ParkingDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    NumberOfPlaces = p.NumberOfPlaces,
                    OpeningTime = p.OpeningTime,
                    ClosingTime = p.ClosingTime,
                    PricePerHour = (int)p.PricePerHour
                })
                .ToList();

            return parkingDtos;
        }

        public async Task<List<ParkingDto>> GetAllParkingsAsync()
        {
            var parkings = await _parkingRepository.GetAllParkingsAsync();

            if (parkings == null)
            {
                throw new KeyNotFoundException($"There are no existing parkings.");
            }

            var parkingDtos = parkings
                 .Select(p => new ParkingDto
                 {
                     Id = p.Id,
                     Name = p.Name,
                     NumberOfPlaces = p.NumberOfPlaces,
                     OpeningTime = p.OpeningTime,
                     ClosingTime = p.ClosingTime,
                     PricePerHour = (int)p.PricePerHour
                 })
                 .ToList();

            return parkingDtos;
        }

        public async Task<ParkingDto> GetParkingAsync(int id)
        {
            var parking = await _parkingRepository.GetParkingAsync(id);

            if (parking == null)
            {
                throw new KeyNotFoundException($"Parking with ID {id} does not exist.");
            }

            return new ParkingDto
            {
                Id = parking.Id,
                Name = parking.Name,
                NumberOfPlaces = parking.NumberOfPlaces,
                OpeningTime = parking.OpeningTime,
                ClosingTime = parking.ClosingTime,
                PricePerHour = (int)parking.PricePerHour
            };
        }

        public async Task<List<ParkingDto>> UpdateParkingAsync(Parking updatedParking)
        {
            var dbParking = await _parkingRepository.GetParkingAsync(updatedParking.Id);

            if (dbParking == null)
                throw new ArgumentException($"Parking with ID {updatedParking.Id} does not exist.");

            if (updatedParking.Name.Length < 3 ||
                updatedParking.NumberOfPlaces <= 0 ||
                updatedParking.PricePerHour <= 0 ||
                updatedParking.ClosingTime <= updatedParking.OpeningTime)
                throw new ArgumentException("Invalid data provided.");


            dbParking.Name = updatedParking.Name;
            dbParking.NumberOfPlaces = updatedParking.NumberOfPlaces;
            dbParking.OpeningTime = updatedParking.OpeningTime;
            dbParking.ClosingTime = updatedParking.ClosingTime;
            dbParking.PricePerHour = (int)updatedParking.PricePerHour;

            await _parkingRepository.UpdateParkingAsync(dbParking);

            var parkings = await _parkingRepository.GetAllParkingsAsync();

            var parkingDtos = parkings
                .Select(p => new ParkingDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    NumberOfPlaces = p.NumberOfPlaces,
                    OpeningTime = p.OpeningTime,
                    ClosingTime = p.ClosingTime,
                    PricePerHour = (int)p.PricePerHour
                })
                .ToList();

            return parkingDtos;
        }
    }
}

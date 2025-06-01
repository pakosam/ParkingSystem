using ParkingSystem.Data;
using ParkingSystem.DTOs;
using ParkingSystem.Entities;
using ParkingSystem.Repositories;

namespace ParkingSystem.Services
{
    public class ParkingService : IParkingService
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly IEmployeeService _employeeService;

        public ParkingService(IParkingRepository parkingRepository, IEmployeeService employeeService)
        {
            _parkingRepository = parkingRepository;
            _employeeService = employeeService;
        }

        public async Task<ParkingDto> CreateParkingAsync(CreateParkingDto parkingDto)
        {
            ValidateAddedParkingData(parkingDto);

            var parking = new Parking
            {
                Name = parkingDto.Name,
                NumberOfPlaces = parkingDto.NumberOfPlaces,
                OpeningTime = parkingDto.OpeningTime,
                ClosingTime = parkingDto.ClosingTime,
                PricePerHour = parkingDto.PricePerHour
            };

            await _parkingRepository.CreateParkingAsync(parking);

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

        public async Task<ParkingDto> DeleteParkingAsync(int id)
        {
            try
            {
                var parking = await _parkingRepository.GetParkingAsync(id);

                if (parking == null)
                {
                    throw new ArgumentException($"Parking with ID {id} does not exist.");
                }

                var employeeIds = await _employeeService.GetEmployeesIdByParkingIdAsync(id);

                await _employeeService.UpdateEmployeesParkingIdAsync(employeeIds);

                await _parkingRepository.DeleteParkingAsync(parking);

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
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                throw; 
            }
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

        public async Task<ParkingDto> UpdateParkingAsync(Parking updatedParking)
        {
            var dbParking = await _parkingRepository.GetParkingAsync(updatedParking.Id);

            if (dbParking == null)
                throw new ArgumentException($"Parking with ID {updatedParking.Id} does not exist.");

            ValidateUpdatedParkingData(updatedParking);

            dbParking.Name = updatedParking.Name;
            dbParking.NumberOfPlaces = updatedParking.NumberOfPlaces;
            dbParking.OpeningTime = updatedParking.OpeningTime;
            dbParking.ClosingTime = updatedParking.ClosingTime;
            dbParking.PricePerHour = (int)updatedParking.PricePerHour;

            await _parkingRepository.UpdateParkingAsync(dbParking);

            return new ParkingDto
            {
                Id = dbParking.Id,
                Name = dbParking.Name,
                NumberOfPlaces = dbParking.NumberOfPlaces,
                OpeningTime = dbParking.OpeningTime,
                ClosingTime = dbParking.ClosingTime,
                PricePerHour = (int)dbParking.PricePerHour
            };
        }

        private void ValidateParkingData(string name, int numberOfPlaces, int pricePerHour, TimeSpan openingTime, TimeSpan closingTime)
        {
            if (name.Length < 3)
                throw new ArgumentException("Parking name must be at least 3 characters long.");
            if (numberOfPlaces <= 0)
                throw new ArgumentException("Number of places must be greater than 0.");
            if (pricePerHour <= 0)
                throw new ArgumentException("Price per hour must be greater than 0.");
            if (closingTime <= openingTime)
                throw new ArgumentException("Closing time must be later than opening time.");
        }

        private void ValidateAddedParkingData(CreateParkingDto parking)
        {
            ValidateParkingData(parking.Name, parking.NumberOfPlaces, (int)parking.PricePerHour, parking.OpeningTime, parking.ClosingTime);
        }

        private void ValidateUpdatedParkingData(Parking parking)
        {
            ValidateParkingData(parking.Name, parking.NumberOfPlaces, (int)parking.PricePerHour, parking.OpeningTime, parking.ClosingTime);
        }
    }
}

using ParkingSystem.DTOs;
using ParkingSystem.Entities;
using ParkingSystem.Repositories;

namespace ParkingSystem.Services
{
    public class ParkingService : IParkingService
    {
        private readonly IParkingRepository _parkingRepository;

        public ParkingService(IParkingRepository parkingRepository)
        {
            _parkingRepository = parkingRepository;
        }

        public async Task<List<ParkingDto>>CreateParkingAsync(CreateParkingDto parkingDto)
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

            return parkings;

        }

        public Task<List<ParkingDto>> DeleteParkingAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ParkingDto>> GetAllParkingsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ParkingDto> GetParkingAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ParkingDto>> UpdateParkingAsync(Parking updatedParking)
        {
            if (updatedParking.Name.Length < 3 ||
                updatedParking.NumberOfPlaces <= 0 ||
                updatedParking.PricePerHour <= 0 ||
                updatedParking.ClosingTime <= updatedParking.OpeningTime)
                throw new NotImplementedException();

            await _parkingRepository.UpdateParkingAsync(updatedParking);

            var parkings = await _parkingRepository.GetAllParkingsAsync();

            return parkings;

        }
    }
}

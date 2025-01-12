namespace ParkingSystem.DTOs
{
    public class ParkingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfPlaces { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public int PricePerHour { get; set; }
    }

    public class CreateParkingDto
    {
        public string Name { get; set; }
        public int NumberOfPlaces { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public int PricePerHour { get; set; }
    }
}
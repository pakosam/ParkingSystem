namespace ParkingSystem.Entities
{
    public class Parking
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfPlaces { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public decimal PricePerHour { get; set; }
    }
}

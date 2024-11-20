namespace ParkingSystem.Entities
{
    public class Parking
    {
        public string Name { get; set; }
        public int NumberOfPlaces { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public int TicketPrice { get; set; }
    }
}

namespace ParkingSystem.Entities
{
    public class ParkingEntry
    {
         public int Id { get; set; }
         public string RegistrationPlate { get; set; }
         public DateTime TicketTakeover { get; set; }
         public DateTime TicketExpiration { get; set; }
         public int ParkingId { get; set; }
         public Parking Parking { get; set; }
    }
}

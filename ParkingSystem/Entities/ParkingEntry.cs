namespace ParkingSystem.Entities
{
    public class ParkingEntry
    {
         public int Id { get; set; }
         public string RegistrationPlate { get; set; }
         public DateTime? TicketTakeover { get; set; }
         public DateTime? TicketExpiration { get; set; }
         public int? ParkingId { get; internal set; }
         public ParkingPayments? Payment { get; set; }
    }
}

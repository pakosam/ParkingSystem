namespace ParkingSystem.Entities
{
    public class ParkingPayments
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int ParkingEntryId { get; set; }
    }
}

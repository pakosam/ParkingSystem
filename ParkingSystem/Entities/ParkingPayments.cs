namespace ParkingSystem.Entities
{
    public class ParkingPayments
    {
        public int Amount { get; set; }
        public int ParkingEntryId { get; set; }
        public ParkingEntry ParkingEntry { get; set; }
    }
}

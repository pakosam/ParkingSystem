using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingSystem.Entities
{
    public enum Currency
    {
        EUR,
        USD,
        GBP
    }

    public class ParkingPayments
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int ParkingEntryId { get; set; }
        public Currency Currency { get; set; } = Currency.EUR;
    }
}

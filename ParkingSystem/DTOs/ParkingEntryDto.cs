using ParkingSystem.DTOs;
using ParkingSystem.Entities;

namespace ParkingSystem.DTOs
{
    public class ParkingEntryDto
    {
        public int Id { get; set; }
        public string RegistrationPlate { get; set; }
        public DateTime? TicketTakeover { get; set; }
        public DateTime? TicketExpiration { get; set; }
        public int? ParkingId { get; set; }
    }
}

public class CreateParkingEntryDto
{
    public string RegistrationPlate { get; set; }
    public DateTime? TicketTakeover { get; set; }
}

public class CreateParkingLeaveDto
{
    public DateTime? TicketExpiration { get; set; }
}
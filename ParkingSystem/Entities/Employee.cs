using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace ParkingSystem.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? ParkingId { get; internal set; }
    }
}
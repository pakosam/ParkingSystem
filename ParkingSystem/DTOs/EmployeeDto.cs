﻿namespace ParkingSystem.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public int ParkingId { get; set; }
    }
}

public class CreateEmployeeDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
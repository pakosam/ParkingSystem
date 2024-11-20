using Microsoft.EntityFrameworkCore;
using ParkingSystem.Entities;

namespace ParkingSystem.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Parking> Parkings { get; set; }
        public DbSet<ParkingEntry> ParkingEntries { get; set; }
        public DbSet<ParkingPayments> ParkingPayments { get; set; }
    }
}

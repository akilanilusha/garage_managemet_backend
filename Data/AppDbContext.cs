using garage_managemet_backend_api.Entitiy;
using garage_managemet_backend_api.Models;
using Microsoft.EntityFrameworkCore;

namespace garage_managemet_backend_api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Appointment> Appointment { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Customer> Customer { get; set; }
    public DbSet<Service> Service { get; set; }
    public DbSet<Mechanic> Mechanic { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<ServiceItem> ServiceItems { get; set; }
    public DbSet<Payment> Payments { get; set; }


}

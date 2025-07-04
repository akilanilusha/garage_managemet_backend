using System;
using garage_managemet_backend_api.Entitiy;
using garage_managemet_backend_api.Models;
using Microsoft.EntityFrameworkCore;

namespace garage_managemet_backend_api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Appointment> Appointment { get; set; }
    public DbSet<Vehicle> Vehicles{ get; set; }
    public DbSet<Customer> Customer{ get; set; }
}

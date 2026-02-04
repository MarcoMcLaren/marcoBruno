using Domain;
using Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    #region DbSets

    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Rental> Rentals { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Vehicle Configuration

        // Primary Key
        modelBuilder.Entity<Vehicle>()
            .HasKey(v => v.VehicleId);

        // Inheritance - Table-per-Hierarchy (TPH)
        modelBuilder.Entity<Vehicle>()
            .HasDiscriminator<string>("VehicleType")
            .HasValue<Car>("Car");
        // Add more types here as needed:
        // .HasValue<SUV>("SUV")
        // .HasValue<Truck>("Truck")

        // Properties
        modelBuilder.Entity<Vehicle>()
            .Property(v => v.DailyBaseRate)
            .HasColumnType("decimal(10,2)");

        // Indexes
        modelBuilder.Entity<Vehicle>()
            .HasIndex(v => v.VIN)
            .IsUnique();

        modelBuilder.Entity<Vehicle>()
            .HasIndex(v => v.Status);

        #endregion

        #region Customer Configuration

        // Primary Key
        modelBuilder.Entity<Customer>()
            .HasKey(c => c.CustomerId);

        // Indexes
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Email)
            .IsUnique();

        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.DriverLicenseNumber)
            .IsUnique();

        #endregion

        #region Rental Configuration

        // Primary Key
        modelBuilder.Entity<Rental>()
            .HasKey(r => r.RentalId);

        // Indexes
        modelBuilder.Entity<Rental>()
            .HasIndex(r => r.Status);

        modelBuilder.Entity<Rental>()
            .HasIndex(r => new { r.StartDate, r.EndDate });

        #endregion

        #region Relationships

        // Rental -> Vehicle (Many-to-One)
        modelBuilder.Entity<Rental>()
            .HasOne(r => r.Vehicle)
            .WithMany(v => v.Rentals)
            .HasForeignKey(r => r.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Rental -> Customer (Many-to-One)
        modelBuilder.Entity<Rental>()
            .HasOne(r => r.Customer)
            .WithMany(c => c.Rentals)
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion
    }
}
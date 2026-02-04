using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Vehicle
{
        // Primary Key
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VehicleId { get; set; }

        // Vehicle Identification
        [Required]
        [StringLength(17, MinimumLength = 17)]
        [RegularExpression(@"^[A-HJ-NPR-Z0-9]{17}$", ErrorMessage = "Invalid VIN format")]
        public string VIN { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string LicensePlate { get; set; } = string.Empty;

        // Vehicle Details
        [Required]
        [StringLength(50)]
        public string Make { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Model { get; set; } = string.Empty;

        [Required]
        [Range(1900, 2100)]
        public int Year { get; set; }

        [Required]
        [StringLength(30)]
        public string Color { get; set; } = string.Empty;

        // Vehicle Metrics
        [Required]
        [Range(0, int.MaxValue)]
        public int Mileage { get; set; }

        [Required]
        [StringLength(20)]
        public string FuelType { get; set; } = string.Empty; // Gasoline, Diesel, Electric, Hybrid

        // Status and Availability
        [Required]
        public VehicleStatus Status { get; set; } = VehicleStatus.Available;

        // Pricing
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, 10000.00)]
        public decimal DailyBaseRate { get; set; }

        // Location and Image
        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        [StringLength(500)]
        public string? ImageURL { get; set; }

        // Maintenance Tracking
        public DateTime? LastServiceDate { get; set; }

        public DateTime? InsuranceExpiry { get; set; }

        // Audit Fields
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
        
}
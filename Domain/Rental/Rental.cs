using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Rental
{
    // Primary Key
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RentalId { get; set; }
    
    // Foreign Keys
    [Required]
    public int VehicleId { get; set; }
    public virtual Vehicle Vehicle { get; set; } = null!;

    [Required]
    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; } = null!;

    // Rental Dates
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime StartDate { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime EndDate { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? ActualReturnDate { get; set; }

    // Location Information
    [Required]
    [StringLength(100)]
    public string PickupLocation { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string ReturnLocation { get; set; } = string.Empty;

    // Mileage Tracking
    [Required]
    [Range(0, int.MaxValue)]
    public int InitialMileage { get; set; }

    [Range(0, int.MaxValue)]
    public int? FinalMileage { get; set; }

    // Rental Status
    [Required]
    public RentalStatus Status { get; set; } = RentalStatus.Reserved;

    // Audit Fields
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public decimal TotalCost { get; set; }

}
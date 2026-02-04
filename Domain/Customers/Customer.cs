using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Customer
{
    // Primary Key
    public int CustomerId { get; set; }

    // Personal Information
    [Required] [StringLength(50)] public string FirstName { get; set; } = string.Empty;

    [Required] [StringLength(50)] public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Phone] [StringLength(20)] public string? PhoneNumber { get; set; }

    [Required] [DataType(DataType.Date)] public DateTime DateOfBirth { get; set; }

    // Driver License Information
    [Required] [StringLength(50)] public string DriverLicenseNumber { get; set; } = string.Empty;

    [Required] [DataType(DataType.Date)] public DateTime DriverLicenseExpiry { get; set; }

    [Required] [StringLength(50)] public string DriverLicenseCountry { get; set; } = "RSA";

    // Address Information
    [StringLength(200)] public string? Address { get; set; }

    [StringLength(100)] public string? City { get; set; }

    [StringLength(50)] public string? Province { get; set; }

    [StringLength(10)] public string? ZipCode { get; set; }

    // Account Information
    [Required] public UserRole UserRole { get; set; } = UserRole.Customer;

    // Audit Fields
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
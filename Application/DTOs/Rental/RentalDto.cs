using Application.DTOs.Customer;
using Application.DTOs.Vehicle;
using Domain;

namespace Application.DTOs.Rental;

public class RentalDto
{
    public int RentalId { get; set; }
    
    // Customer info
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    
    // Vehicle info
    public int VehicleId { get; set; }
    public string VehicleMake { get; set; } = string.Empty;
    public string VehicleModel { get; set; } = string.Empty;
    public int VehicleYear { get; set; }
    public string VehicleLicensePlate { get; set; } = string.Empty;
    
    // Rental details
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? ActualReturnDate { get; set; }
    
    public string PickupLocation { get; set; } = string.Empty;
    public string ReturnLocation { get; set; } = string.Empty;
    
    public int? InitialMileage { get; set; }
    public int? FinalMileage { get; set; }
    
    public decimal DailyRate { get; set; }
    public decimal? TotalCost { get; set; }
    
    public RentalStatus Status { get; set; }
    
    // Audit
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Computed properties
    public int TotalDays => (EndDate - StartDate).Days;
    public int? ActualDays => ActualReturnDate.HasValue ? (ActualReturnDate.Value - StartDate).Days : null;
    public bool IsOverdue => Status == RentalStatus.Active && DateTime.Now > EndDate;
    
    public CarDto VehicleDto { get; set; } 
    public CustomerDto CustomerDto { get; set; }
    
}
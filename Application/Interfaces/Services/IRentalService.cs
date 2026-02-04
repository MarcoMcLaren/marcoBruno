using Application.DTOs.Rental;
using Domain;

namespace Application.Services;

public interface IRentalService
{
    // READ (DTOs)
    Task<RentalDto?> GetRentalByIdAsync(int id);
    Task<IEnumerable<RentalDto>> GetAllRentalsAsync();
    Task<IEnumerable<RentalDto>> GetActiveRentalsAsync();
    Task<IEnumerable<RentalDto>> GetRentalsByCustomerIdAsync(int customerId);
    Task<IEnumerable<RentalDto>> GetRentalsByVehicleIdAsync(int vehicleId);

    // WRITE (Requests)
    Task<RentalDto> CreateRentalAsync(RentalDto request);
    Task UpdateRentalAsync(int id, RentalDto request);
    Task DeleteRentalAsync(int id); // Cancel rental (soft delete)
    
    // Business operations
    Task<bool> CanCustomerRentAsync(int customerId);
    Task<bool> IsVehicleAvailableAsync(int vehicleId, DateTime startDate, DateTime endDate);
    Task<RentalDto> ActivateRentalAsync(int rentalId, int initialMileage);
    Task<RentalDto> CompleteRentalAsync(int rentalId, int finalMileage, DateTime actualReturnDate);
}
using Domain;

namespace Application.Repositories;

public interface IRentalRepository
{
    // Query operations
    Task<Rental?> GetByIdAsync(int id);
    Task<IEnumerable<Rental>> GetAllAsync();
    Task<IEnumerable<Rental>> GetActiveRentalsAsync();
    Task<IEnumerable<Rental>> GetRentalsByCustomerIdAsync(int customerId);
    Task<IEnumerable<Rental>> GetRentalsByVehicleIdAsync(int vehicleId);
    Task<IEnumerable<Rental>> GetRentalsByStatusAsync(RentalStatus status);
    Task<IEnumerable<Rental>> GetRentalsWithDetailsAsync(); // Includes Customer and Vehicle

    // Command operations
    Task<Rental> AddAsync(Rental rental);
    Task UpdateAsync(Rental rental);
    Task DeleteAsync(int id); // Soft delete (cancel rental)
    Task<bool> ExistsAsync(int id);
    
    // Business queries
    Task<bool> HasActiveRentalAsync(int customerId);
    Task<bool> IsVehicleAvailableAsync(int vehicleId, DateTime startDate, DateTime endDate);
}
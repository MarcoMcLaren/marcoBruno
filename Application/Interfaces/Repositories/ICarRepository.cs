using Application.DTOs.Vehicle;
using Domain;
using Domain.Vehicles;

namespace Application.Repositories;

public interface ICarRepository
{
    // READ (DTOs)
    Task<Car?> GetByIdAsync(int id);
    Task<Car?> GetByVinAsync(string vin);
    Task<IEnumerable<Car>> GetAllAsync();
    Task<IEnumerable<Car>> GetAvailableAsync();

    // WRITE (DTO in your current approach)
    Task<Car> AddAsync(Car request);
    Task UpdateAsync( Car request);
    Task DeleteAsync(int id); // soft delete / deactivate
    
    Task<bool> ExistsAsync(int id);
    Task<bool> HasAnyRentalsAsync(int vehicleId);
}
using Application.DTOs.Vehicle;

namespace Application.Services;

public interface ICarService
{
    // READ (DTOs)
    Task<CarDto?> GetCarByIdAsync(int id);
    Task<CarDto?> GetCarByVINAsync(string vin);
    Task<IEnumerable<CarDto>> GetAllCarsAsync();
    Task<IEnumerable<CarDto>> GetActiveCarsAsync();
    Task<IEnumerable<CarDto>> GetAvailableCarsAsync();
    
    // WRITE (DTOs, not domain entities)
    Task<CarDto> CreateCarsAsync(CarDto request);
    Task UpdateCarAsync(int id, CarDto request);
    Task DeleteCarAsync(int id);
}
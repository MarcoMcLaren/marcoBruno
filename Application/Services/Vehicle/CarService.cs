using Application.DTOs.Vehicle;
using Application.Repositories;
using AutoMapper;
using Domain;
using Domain.Vehicles;

namespace Application.Services.Vehicle;

public class CarService : ICarService
{
    private readonly ICarRepository _repo;
    private readonly IMapper _mapper;

    public CarService(ICarRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }
    
    public async Task<CarDto?> GetCarByIdAsync(int id)
    {
        var car = await _repo.GetByIdAsync(id);
        return car == null ? null : _mapper.Map<CarDto>(car);
    }

    public async Task<CarDto?> GetCarByVINAsync(string vin)
    {
        var car = await _repo.GetByVinAsync(vin);
        return car == null ? null : _mapper.Map<CarDto>(car);
    }

    public async Task<IEnumerable<CarDto>> GetAllCarsAsync()
    {
        var cars = await _repo.GetAllAsync();
        return cars.Select(c => _mapper.Map<CarDto>(c));
    }

    public async Task<IEnumerable<CarDto>> GetActiveCarsAsync()
    {
        var cars = await _repo.GetAvailableAsync();
        return cars.Select(c => _mapper.Map<CarDto>(c));
    }

    public async Task<CarDto> CreateCarsAsync(CarDto request)
    {
        // ADDED: DTO -> Domain
        var car = _mapper.Map<Car>(request);

        // ADDED: system-controlled defaults (adjust as needed)
        car.Status = car.Status == default ? VehicleStatus.Available : car.Status;

        var created = await _repo.AddAsync(car);
        return _mapper.Map<CarDto>(created);
    }

    public async Task UpdateCarAsync(int id, CarDto request)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException("Car not found."); // ADDED
        
        // ADDED: map allowed scalars onto existing entity
        _mapper.Map(request, existing); // requires mapping config below
    
        // ADDED: never allow update to overwrite identity key
        existing.VehicleId = id; // ADDED safety

        await _repo.UpdateAsync( existing);
    }

    public async Task DeleteCarAsync(int id)
    {
        if (!await _repo.ExistsAsync(id))
            throw new KeyNotFoundException("Car not found."); // ADDED

        // ADDED: rule - cannot delete/deactivate if it has rentals (you can adjust to only “active rentals”)
        if (await _repo.HasAnyRentalsAsync(id))
            throw new InvalidOperationException("Cannot delete/deactivate a car that has rentals."); // ADDED

        await _repo.DeleteAsync(id);
    }
    
    public async Task<IEnumerable<CarDto>> GetAvailableCarsAsync()
    {
        var cars = await _repo.GetAvailableAsync();
        return cars.Select(c => _mapper.Map<CarDto>(c));
    }
}
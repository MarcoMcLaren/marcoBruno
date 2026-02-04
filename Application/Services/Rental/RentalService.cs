using Application.DTOs.Rental;
using Application.Repositories;
using AutoMapper;
using Domain;

namespace Application.Services.Rental;

public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ICarRepository _carRepository;
    private readonly IMapper _mapper;

    public RentalService(
        IRentalRepository rentalRepository,
        ICustomerRepository customerRepository,
        ICarRepository carRepository, IMapper mapper)
    {
        _rentalRepository = rentalRepository;
        _customerRepository = customerRepository;
        _carRepository = carRepository;
        _mapper = mapper;
    }

    #region READ Operations

    public async Task<RentalDto?> GetRentalByIdAsync(int id)
    {
        var rental = await _rentalRepository.GetByIdAsync(id);
        return rental != null ? MapToDto(rental) : null;
    }

    public async Task<IEnumerable<RentalDto>> GetAllRentalsAsync()
    {
        var rentals = await _rentalRepository.GetAllAsync();
        return rentals.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<RentalDto>> GetActiveRentalsAsync()
    {
        var rentals = await _rentalRepository.GetActiveRentalsAsync();
        return rentals.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<RentalDto>> GetRentalsByCustomerIdAsync(int customerId)
    {
        var rentals = await _rentalRepository.GetRentalsByCustomerIdAsync(customerId);
        return rentals.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<RentalDto>> GetRentalsByVehicleIdAsync(int vehicleId)
    {
        var rentals = await _rentalRepository.GetRentalsByVehicleIdAsync(vehicleId);
        return rentals.Select(MapToDto).ToList();
    }

    #endregion

    #region WRITE Operations

    public async Task<RentalDto> CreateRentalAsync(RentalDto request)
    {
        // Business validation
        
        // 1. Validate customer exists and is active
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
        if (customer == null || !customer.IsActive)
        {
            throw new InvalidOperationException("Customer not found or inactive.");
        }

        // 2. Validate customer can rent (no active rentals, license valid, age 21+)
        if (!await CanCustomerRentAsync(request.CustomerId))
        {
            throw new InvalidOperationException("Customer cannot rent at this time. Check for active rentals, valid license, or age requirement.");
        }

        // 3. Validate vehicle exists and is active
        var vehicle = await _carRepository.GetByIdAsync(request.VehicleId);
        if (vehicle == null || !vehicle.IsActive)
        {
            throw new InvalidOperationException("Vehicle not found or inactive.");
        }

        // 4. Validate vehicle is available for the requested dates
        if (!await IsVehicleAvailableAsync(request.VehicleId, request.StartDate, request.EndDate))
        {
            throw new InvalidOperationException("Vehicle is not available for the selected dates.");
        }

        // 5. Validate dates
        if (request.StartDate < DateTime.Today)
        {
            throw new InvalidOperationException("Start date cannot be in the past.");
        }

        if (request.EndDate <= request.StartDate)
        {
            throw new InvalidOperationException("End date must be after start date.");
        }

        // Set initial values
        request.CreatedDate = DateTime.UtcNow;
        request.Status = RentalStatus.Reserved;

        // Calculate total cost
        var days = (request.EndDate - request.StartDate).Days;
        request.TotalCost = days * vehicle.DailyBaseRate;
        
        var rentalCar = _mapper.Map<Domain.Rental>(request);
        rentalCar.CustomerId = request.CustomerId;
        rentalCar.Customer = customer;
        rentalCar.Vehicle = vehicle;
        rentalCar.VehicleId = request.VehicleId;
        var createdRental = await _rentalRepository.AddAsync(rentalCar);
        
        // Update vehicle status to Reserved
        vehicle.Status = VehicleStatus.Reserved;
        await _carRepository.UpdateAsync(vehicle);

        return MapToDto(createdRental);
    }

    public async Task UpdateRentalAsync(int id, RentalDto request)
    {
        var existingRental = await _rentalRepository.GetByIdAsync(id);
        if (existingRental == null)
        {
            throw new KeyNotFoundException($"Rental with ID {id} not found.");
        }

        // Can only update certain fields based on status
        if (existingRental.Status == RentalStatus.Completed || existingRental.Status == RentalStatus.Cancelled)
        {
            throw new InvalidOperationException("Cannot update completed or cancelled rentals.");
        }

        // Update allowed fields
        existingRental.StartDate = request.StartDate;
        existingRental.EndDate = request.EndDate;
        existingRental.PickupLocation = request.PickupLocation;
        existingRental.ReturnLocation = request.ReturnLocation;

        // Recalculate total cost if dates changed
        var days = (existingRental.EndDate - existingRental.StartDate).Days;
        existingRental.TotalCost = days * existingRental.Vehicle.DailyBaseRate;

        await _rentalRepository.UpdateAsync(existingRental);
    }

    public async Task DeleteRentalAsync(int id)
    {
        var rental = await _rentalRepository.GetByIdAsync(id);
        if (rental == null)
        {
            throw new KeyNotFoundException($"Rental with ID {id} not found.");
        }

        // Can only cancel Reserved or Active rentals
        if (rental.Status == RentalStatus.Completed)
        {
            throw new InvalidOperationException("Cannot cancel a completed rental.");
        }

        // Update vehicle status back to Available
        var vehicle = await _carRepository.GetByIdAsync(rental.VehicleId);
        if (vehicle != null)
        {
            vehicle.Status = VehicleStatus.Available;
            await _carRepository.UpdateAsync(vehicle);
        }

        await _rentalRepository.DeleteAsync(id);
    }

    #endregion

    #region Business Operations

    public async Task<bool> CanCustomerRentAsync(int customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null || !customer.IsActive)
        {
            return false;
        }

        // Check age (must be 21+)
        var age = DateTime.Today.Year - customer.DateOfBirth.Year;
        if (customer.DateOfBirth.Date > DateTime.Today.AddYears(-age))
        {
            age--;
        }

        if (age < 21)
        {
            return false;
        }

        // Check license validity
        if (customer.DriverLicenseExpiry <= DateTime.Today)
        {
            return false;
        }

        // Check for active rentals
        if (await _rentalRepository.HasActiveRentalAsync(customerId))
        {
            return false;
        }

        return true;
    }

    public async Task<bool> IsVehicleAvailableAsync(int vehicleId, DateTime startDate, DateTime endDate)
    {
        return await _rentalRepository.IsVehicleAvailableAsync(vehicleId, startDate, endDate);
    }

    public async Task<RentalDto> ActivateRentalAsync(int rentalId, int initialMileage)
    {
        var rental = await _rentalRepository.GetByIdAsync(rentalId);
        if (rental == null)
        {
            throw new KeyNotFoundException($"Rental with ID {rentalId} not found.");
        }

        if (rental.Status != RentalStatus.Reserved)
        {
            throw new InvalidOperationException("Can only activate reserved rentals.");
        }

        // Update rental status
        rental.Status = RentalStatus.Active;
        rental.InitialMileage = initialMileage;

        await _rentalRepository.UpdateAsync(rental);

        // Update vehicle status
        var vehicle = await _carRepository.GetByIdAsync(rental.VehicleId);
        if (vehicle != null)
        {
            vehicle.Status = VehicleStatus.Rented;
            vehicle.Mileage = initialMileage;
            await _carRepository.UpdateAsync(vehicle);
        }

        return MapToDto(rental);
    }

    public async Task<RentalDto> CompleteRentalAsync(int rentalId, int finalMileage, DateTime actualReturnDate)
    {
        var rental = await _rentalRepository.GetByIdAsync(rentalId);
        if (rental == null)
        {
            throw new KeyNotFoundException($"Rental with ID {rentalId} not found.");
        }

        if (rental.Status != RentalStatus.Active)
        {
            throw new InvalidOperationException("Can only complete active rentals.");
        }

        // Update rental
        rental.Status = RentalStatus.Completed;
        rental.FinalMileage = finalMileage;
        rental.ActualReturnDate = actualReturnDate;

        // Recalculate cost if returned late
        var actualDays = (actualReturnDate - rental.StartDate).Days;
        if (actualDays > (rental.EndDate - rental.StartDate).Days)
        {
            rental.TotalCost = actualDays * rental.Vehicle.DailyBaseRate;
        }

        await _rentalRepository.UpdateAsync(rental);

        // Update vehicle
        var vehicle = await _carRepository.GetByIdAsync(rental.VehicleId);
        if (vehicle != null)
        {
            vehicle.Status = VehicleStatus.Available;
            vehicle.Mileage = finalMileage;
            await _carRepository.UpdateAsync(vehicle);
        }

        return MapToDto(rental);
    }

    public async Task<decimal> CalculateTotalCostAsync(int rentalId)
    {
        var rental = await _rentalRepository.GetByIdAsync(rentalId);
        if (rental == null)
        {
            throw new KeyNotFoundException($"Rental with ID {rentalId} not found.");
        }

        var endDate = rental.ActualReturnDate ?? rental.EndDate;
        var days = (endDate - rental.StartDate).Days;

        return days * rental.Vehicle.DailyBaseRate;
    }

    #endregion

    #region Helper Methods

    private static RentalDto MapToDto(Domain.Rental rental)
    {
        return new RentalDto
        {
            RentalId = rental.RentalId,
            CustomerId = rental.CustomerId,
            CustomerName = $"{rental.Customer.FirstName} {rental.Customer.LastName}",
            CustomerEmail = rental.Customer.Email,
            VehicleId = rental.VehicleId,
            VehicleMake = rental.Vehicle.Make,
            VehicleModel = rental.Vehicle.Model,
            VehicleYear = rental.Vehicle.Year,
            VehicleLicensePlate = rental.Vehicle.LicensePlate,
            StartDate = rental.StartDate,
            EndDate = rental.EndDate,
            ActualReturnDate = rental.ActualReturnDate,
            PickupLocation = rental.PickupLocation,
            ReturnLocation = rental.ReturnLocation,
            InitialMileage = rental.InitialMileage,
            FinalMileage = rental.FinalMileage,
            DailyRate = rental.Vehicle.DailyBaseRate,
            TotalCost = rental.TotalCost,
            Status = rental.Status,
            CreatedDate = rental.CreatedDate
        };
    }

    #endregion
}
using Application.Repositories;
using Domain;
using Domain.Vehicles;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CarRepository : ICarRepository
    {
        private readonly ApplicationDbContext _db;

        public CarRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        #region Queries

        public async Task<Car?> GetByIdAsync(int id)
        {
            // NOTE: Cars is a DbSet<Car> in your DbContext
            return await _db.Cars.FirstOrDefaultAsync(c => c.VehicleId == id);
        }

        public async Task<Car?> GetByVinAsync(string vin)
        {
            return await _db.Cars.FirstOrDefaultAsync(c => c.VIN == vin);
        }

        public async Task<IEnumerable<Car>> GetAllAsync()
        {
            return await _db.Cars
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetAvailableAsync()
        {
            // CHANGED/NOTE: assumes you have a Status property on Vehicle/Car (like Available/Rented/etc.)
            // If your status names differ, adjust here.
            return await _db.Cars
                .AsNoTracking()
                .Where(c => c.Status == VehicleStatus.Available) 
                .ToListAsync();
        }

        public async Task<bool> VinExistsAsync(string vin)
        {
            return await _db.Cars.AnyAsync(c => c.VIN == vin);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Cars.AnyAsync(c => c.VehicleId == id);
        }

        public async Task<bool> HasAnyRentalsAsync(int vehicleId)
        {
            // Rentals point to VehicleId, so check Rentals set
            return await _db.Rentals.AnyAsync(r => r.VehicleId == vehicleId);
        }

        #endregion

        #region Commands

        public async Task<Car> AddAsync(Car car)
        {
            await _db.Cars.AddAsync(car);
            await _db.SaveChangesAsync();
            return car;
        }

        public async Task UpdateAsync( Car car)
        {
            _db.Cars.Update(car);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var car = await _db.Cars.FirstOrDefaultAsync(c => c.VehicleId == id);
            if (car == null) return;

            // ADDED: Soft delete approach for vehicles
            // You likely have Status, so we deactivate rather than delete the row.
            car.Status = VehicleStatus.Retired; // EDIT: choose your actual status value
            _db.Cars.Update(car);
            await _db.SaveChangesAsync();
        }

        #endregion

        
    }
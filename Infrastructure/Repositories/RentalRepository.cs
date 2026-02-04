using Application.Repositories;
using Domain;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RentalRepository : IRentalRepository
{
    private readonly ApplicationDbContext _db;

    public RentalRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    #region Queries

    public async Task<Rental?> GetByIdAsync(int id)
    {
        return await _db.Rentals
            .Include(r => r.Customer)
            .Include(r => r.Vehicle)
            .FirstOrDefaultAsync(r => r.RentalId == id);
    }

    public async Task<IEnumerable<Rental>> GetAllAsync()
    {
        return await _db.Rentals
            .Include(r => r.Customer)
            .Include(r => r.Vehicle)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Rental>> GetActiveRentalsAsync()
    {
        return await _db.Rentals
            .Include(r => r.Customer)
            .Include(r => r.Vehicle)
            .AsNoTracking()
            .Where(r => r.Status == RentalStatus.Active)
            .ToListAsync();
    }

    public async Task<IEnumerable<Rental>> GetRentalsByCustomerIdAsync(int customerId)
    {
        return await _db.Rentals
            .Include(r => r.Customer)
            .Include(r => r.Vehicle)
            .AsNoTracking()
            .Where(r => r.CustomerId == customerId)
            .OrderByDescending(r => r.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Rental>> GetRentalsByVehicleIdAsync(int vehicleId)
    {
        return await _db.Rentals
            .Include(r => r.Customer)
            .Include(r => r.Vehicle)
            .AsNoTracking()
            .Where(r => r.VehicleId == vehicleId)
            .OrderByDescending(r => r.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Rental>> GetRentalsByStatusAsync(RentalStatus status)
    {
        return await _db.Rentals
            .Include(r => r.Customer)
            .Include(r => r.Vehicle)
            .AsNoTracking()
            .Where(r => r.Status == status)
            .ToListAsync();
    }

    public async Task<IEnumerable<Rental>> GetRentalsWithDetailsAsync()
    {
        return await _db.Rentals
            .Include(r => r.Customer)
            .Include(r => r.Vehicle)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> IsVehicleAvailableAsync(int vehicleId, DateTime startDate, DateTime endDate)
    {
        // Check if there are any overlapping rentals
        var hasConflict = await _db.Rentals
            .AnyAsync(r =>
                r.VehicleId == vehicleId &&
                (r.Status == RentalStatus.Reserved || r.Status == RentalStatus.Active) &&
                ((r.StartDate <= endDate && r.EndDate >= startDate))
            );

        return !hasConflict;
    }

    public async Task<bool> HasActiveRentalAsync(int customerId)
    {
        return await _db.Rentals
            .AnyAsync(r =>
                r.CustomerId == customerId &&
                (r.Status == RentalStatus.Reserved || r.Status == RentalStatus.Active)
            );
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _db.Rentals.AnyAsync(r => r.RentalId == id);
    }

    #endregion

    #region Commands

    public async Task<Rental> AddAsync(Rental rental)
    {
        await _db.Rentals.AddAsync(rental);
        await _db.SaveChangesAsync();
        return rental;
    }

    public async Task UpdateAsync(Rental rental)
    {
        _db.Rentals.Update(rental);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var rental = await _db.Rentals.FirstOrDefaultAsync(r => r.RentalId == id);
        if (rental == null) return;

        // ADDED: soft delete (cancel rental)
        rental.Status = RentalStatus.Cancelled;
        _db.Rentals.Update(rental);
        await _db.SaveChangesAsync();
    }

    #endregion
}
using Application.Repositories;

using Domain;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _db;

    public CustomerRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    #region Queries

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _db.Customers.FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _db.Customers.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Customer>> GetActiveCustomersAsync()
    {
        return await _db.Customers.AsNoTracking()
            .Where(c => c.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Customer>> GetCustomersWithRentalsAsync()
    {
        return await _db.Customers.AsNoTracking()
            .Include(c => c.Rentals)
            .ToListAsync();
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _db.Customers.AnyAsync(c => c.Email == email);
    }

    public async Task<bool> DriverLicenseExistsAsync(string licenseNumber)
    {
        return await _db.Customers.AnyAsync(c => c.DriverLicenseNumber == licenseNumber);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _db.Customers.AnyAsync(c => c.CustomerId == id);
    }

    #endregion
    
    #region Commands
    
    public async Task<Customer> AddAsync(Customer customer)
    {
        await _db.Customers.AddAsync(customer);
        await _db.SaveChangesAsync();
        return customer;
    }

    public async Task UpdateAsync(Customer customer)
    {
        _db.Customers.Update(customer);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
        if (customer == null) return;

        // ADDED: soft delete
        customer.IsActive = false; // ADDED
        _db.Customers.Update(customer);
        await _db.SaveChangesAsync();
    }

    #endregion
    
}
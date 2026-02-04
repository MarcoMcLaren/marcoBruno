using Application.DTOs.Customer;
using Domain;

namespace Application.Repositories;

public interface ICustomerRepository
{
    // Query operations
    Task<Customer?> GetByIdAsync(int id);
    Task<Customer?> GetByEmailAsync(string email);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<IEnumerable<Customer>> GetActiveCustomersAsync();
    Task<IEnumerable<Customer>> GetCustomersWithRentalsAsync();
    Task<bool> EmailExistsAsync(string email);
    Task<bool> DriverLicenseExistsAsync(string licenseNumber);

    // Command operations
    Task<Customer> AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(int id); // Soft delete
    Task<bool> ExistsAsync(int id);
}
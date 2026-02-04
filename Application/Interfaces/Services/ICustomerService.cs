using Application.DTOs.Customer;
using Domain;

namespace Application.Services;

public interface ICustomerService
{
    // READ (DTOs)
    Task<CustomerDto?> GetCustomerByIdAsync(int id);
    Task<CustomerDto?> GetCustomerByEmailAsync(string email);
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
    Task<IEnumerable<CustomerDto>> GetActiveCustomersAsync();

    // WRITE (Requests)
    Task<CustomerDto> CreateCustomerAsync(CustomerDto request);
    Task UpdateCustomerAsync(int id, CustomerDto request);
    Task DeleteCustomerAsync(int id); // soft delete
}
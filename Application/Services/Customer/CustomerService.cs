using Application.DTOs.Customer;
using Application.Repositories;
using AutoMapper;
using Domain;

namespace Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repo;
    private readonly IMapper _mapper;

    public CustomerService(ICustomerRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    #region Queries

    public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
    {
        var customer = await _repo.GetByIdAsync(id);
        return customer == null ? null : _mapper.Map<CustomerDto>(customer);
    }

    public async Task<CustomerDto?> GetCustomerByEmailAsync(string email)
    {
        var customer = await _repo.GetByEmailAsync(email);
        return customer == null ? null : _mapper.Map<CustomerDto>(customer);
    }

    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        var customers = await _repo.GetAllAsync();
        return customers.Select(c => _mapper.Map<CustomerDto>(c));
    }

    public async Task<IEnumerable<CustomerDto>> GetActiveCustomersAsync()
    {
        var customers = await _repo.GetActiveCustomersAsync();
        return customers.Select(c => _mapper.Map<CustomerDto>(c));
    }
    #endregion

    #region Commands

    public async Task<CustomerDto> CreateCustomerAsync(CustomerDto request)
    {
        // ADDED: business rule - unique email
        if (await _repo.EmailExistsAsync(request.Email))
            throw new InvalidOperationException("Email already exists."); // ADDED

        // ADDED: business rule - unique driver license (if you have it on request)
        if (!string.IsNullOrWhiteSpace(request.DriverLicenseNumber) &&
            await _repo.DriverLicenseExistsAsync(request.DriverLicenseNumber))
            throw new InvalidOperationException("Driver license number already exists."); // ADDED

        // ADDED: map request -> domain entity
        var customer = _mapper.Map<Customer>(request);

        // ADDED: ensure active by default if your system expects that
        customer.IsActive = true; // EDIT/DELETE if not desired

        var created = await _repo.AddAsync(customer);
        return _mapper.Map<CustomerDto>(created);
    }

    public async Task UpdateCustomerAsync(int id, CustomerDto request)
    {
        // ADDED: existence check
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException("Customer not found."); // ADDED

        // ADDED: uniqueness checks when changing email/license
        if (!string.Equals(existing.Email, request.Email, StringComparison.OrdinalIgnoreCase))
        {
            if (await _repo.EmailExistsAsync(request.Email))
                throw new InvalidOperationException("Email already exists."); // ADDED
        }

        if (!string.IsNullOrWhiteSpace(request.DriverLicenseNumber) &&
            !string.Equals(existing.DriverLicenseNumber, request.DriverLicenseNumber, StringComparison.OrdinalIgnoreCase))
        {
            if (await _repo.DriverLicenseExistsAsync(request.DriverLicenseNumber))
                throw new InvalidOperationException("Driver license number already exists."); // ADDED
        }

        // ADDED: map allowed fields onto existing entity
        _mapper.Map(request, existing); // ADDED (needs mapping config below)

        await _repo.UpdateAsync(existing);
    }

    public async Task DeleteCustomerAsync(int id)
    {
        // ADDED: soft delete happens in repo
        if (!await _repo.ExistsAsync(id))
            throw new KeyNotFoundException("Customer not found."); // ADDED

        await _repo.DeleteAsync(id);
    }

    #endregion
}
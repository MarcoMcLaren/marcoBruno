using Application.DTOs.Customer;
using Application.Services;
using Domain;
using Microsoft.AspNetCore.Components;

namespace Marco_Bruno.Components.Pages;

public partial class Customers : ComponentBase
{
    [Inject] protected ICustomerService CustomerService { get; set; } = null!;

    // =========================
    // ADDED: UI State
    // =========================
    protected List<CustomerDto> CustomerDto { get; set; } = new();
    protected bool IsLoading { get; set; } = true;
    protected bool IsBusy { get; set; }
    protected string? ErrorMessage { get; set; }

    protected bool ShowForm { get; set; }
    protected bool IsEditMode { get; set; }
    protected int EditingCustomerId { get; set; }

    protected Customer FormModel { get; set; } = new();

    // =========================
    // ADDED: Lifecycle
    // =========================
    protected override async Task OnInitializedAsync()
    {
        await LoadCustomers();
    }

    // =========================
    // ADDED: Data loading
    // =========================
    protected async Task LoadCustomers()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;

            CustomerDto = (await CustomerService.GetAllCustomersAsync()).ToList();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    // =========================
    // ADDED: UI Actions
    // =========================
    protected void ShowCreate()
    {
        ShowForm = true;
        IsEditMode = false;
        EditingCustomerId = 0;

        FormModel = new Customer
        {
            IsActive = true
        };
    }

    protected void EditCustomer(CustomerDto dto)
    {
        ShowForm = true;
        IsEditMode = true;
        EditingCustomerId = dto.CustomerId;

        FormModel = new Customer
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            IsActive = dto.IsActive
        };
    }

    protected void CancelForm()
    {
        ShowForm = false;
        IsEditMode = false;
        EditingCustomerId = 0;
        ErrorMessage = null;
    }

    // =========================
    // ADDED: Save
    // =========================
    protected async Task HandleValidSubmit()
    {
        try
        {
            IsBusy = true;
            ErrorMessage = null;

            // Map DTO to Domain entity
            var customer = new CustomerDto()
            {
                FirstName = FormModel.FirstName,
                LastName = FormModel.LastName,
                Email = FormModel.Email,
                PhoneNumber = FormModel.PhoneNumber,
                DriverLicenseNumber = FormModel.DriverLicenseNumber,
                IsActive = FormModel.IsActive
            };

            if (IsEditMode)
            {
                customer.CustomerId = EditingCustomerId;
                await CustomerService.UpdateCustomerAsync(EditingCustomerId, customer);
            }
            else
            {
                await CustomerService.CreateCustomerAsync(customer);
            }

            await LoadCustomers();
            CancelForm();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    // =========================
    // ADDED: Soft delete
    // =========================
    protected async Task SoftDeleteCustomer(int id)
    {
        try
        {
            IsBusy = true;
            ErrorMessage = null;

            await CustomerService.DeleteCustomerAsync(id);
            await LoadCustomers();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
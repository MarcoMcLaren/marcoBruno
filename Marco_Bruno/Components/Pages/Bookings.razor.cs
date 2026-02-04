using Application.DTOs.Customer;
using Application.DTOs.Rental;
using Application.DTOs.Vehicle;
using Application.Services;
using Domain;
using Microsoft.AspNetCore.Components;

namespace Marco_Bruno.Components.Pages;

public partial class Bookings : ComponentBase
{
    [Inject] private IRentalService RentalService { get; set; } = default!;
    [Inject] private ICustomerService CustomerService { get; set; } = default!;
    [Inject] private ICarService CarService { get; set; } = default!;

    private List<RentalDto> RentalDto { get; set; } = new();
    private List<CustomerDto> AvailableCustomers { get; set; } = new();
    private List<CarDto> AvailableVehicles { get; set; } = new();
    private RentalDto FormModel { get; set; } = new();

    private bool IsLoading { get; set; } = true;
    private bool IsBusy { get; set; }
    private bool ShowForm { get; set; }
    private bool IsEditMode { get; set; }
    private string? ErrorMessage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadRentals();
        await LoadCustomers();
        await LoadVehicles();
    }

    private async Task LoadRentals()
    {
        IsLoading = true;
        ErrorMessage = null;

        try
        {
            var rentals = await RentalService.GetAllRentalsAsync();
            RentalDto = rentals.ToList();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading rentals: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task LoadCustomers()
    {
        try
        {
            var customers = await CustomerService.GetActiveCustomersAsync();
            AvailableCustomers = customers.ToList();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading customers: {ex.Message}";
        }
    }

    private async Task LoadVehicles()
    {
        try
        {
            // Get only available vehicles for new rentals
            var vehicles = await CarService.GetAvailableCarsAsync();
            AvailableVehicles = vehicles.ToList();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading vehicles: {ex.Message}";
        }
    }

    private void ShowCreate()
    {
        IsEditMode = false;
        ShowForm = true;
        FormModel = new RentalDto
        {
            StartDate = DateTime.Today.AddDays(1),
            EndDate = DateTime.Today.AddDays(2),
            Status = RentalStatus.Reserved,
            IsActive = true,
            PickupLocation = string.Empty,
            ReturnLocation = string.Empty
        };
    }

    private void EditRental(RentalDto dto)
    {
        IsEditMode = true;
        ShowForm = true;

        // Copy DTO for editing
        FormModel = new RentalDto
        {
            RentalId = dto.RentalId,
            CustomerId = dto.CustomerId,
            VehicleId = dto.VehicleId,
            CustomerName = dto.CustomerName,
            CustomerEmail = dto.CustomerEmail,
            VehicleMake = dto.VehicleMake,
            VehicleModel = dto.VehicleModel,
            VehicleYear = dto.VehicleYear,
            VehicleLicensePlate = dto.VehicleLicensePlate,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            ActualReturnDate = dto.ActualReturnDate,
            PickupLocation = dto.PickupLocation,
            ReturnLocation = dto.ReturnLocation,
            InitialMileage = dto.InitialMileage,
            FinalMileage = dto.FinalMileage,
            DailyRate = dto.DailyRate,
            TotalCost = dto.TotalCost,
            Status = dto.Status,
            IsActive = dto.IsActive
        };
    }

    private async Task HandleValidSubmit()
    {
        IsBusy = true;
        ErrorMessage = null;

        try
        {
            // Validate customer and vehicle selected
            if (FormModel.CustomerId == 0)
            {
                ErrorMessage = "Please select a customer.";
                IsBusy = false;
                return;
            }

            if (FormModel.VehicleId == 0)
            {
                ErrorMessage = "Please select a vehicle.";
                IsBusy = false;
                return;
            }

            if (IsEditMode)
            {
                await RentalService.UpdateRentalAsync(FormModel.RentalId, FormModel);
            }
            else
            {
                await RentalService.CreateRentalAsync(FormModel);
            }

            ShowForm = false;
            await LoadRentals();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error saving rental: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void CancelForm()
    {
        ShowForm = false;
        FormModel = new RentalDto();
    }

    private async Task SoftDeleteRental(int id)
    {
        IsBusy = true;
        ErrorMessage = null;

        try
        {
            await RentalService.DeleteRentalAsync(id);
            await LoadRentals();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error cancelling rental: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static string GetStatusBadgeClass(RentalStatus status)
    {
        return status switch
        {
            RentalStatus.Reserved => "bg-warning text-dark",
            RentalStatus.Active => "bg-primary",
            RentalStatus.Completed=> "bg-success",
            RentalStatus.Cancelled => "bg-secondary",
            _ => "bg-dark"
        };
    }
}
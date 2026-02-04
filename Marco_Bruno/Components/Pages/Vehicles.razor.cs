using Application.DTOs.Vehicle;
using Application.Services;
using AutoMapper;
using Domain;
using Domain.Vehicles;
using Microsoft.AspNetCore.Components;

namespace Marco_Bruno.Components.Pages;

public partial class Vehicles : ComponentBase
{
    [Inject] private ICarService CarService { get; set; } = default!;
    [Inject] private IMapper Mapper { get; set; } = null!;

    private List<CarDto> VehicleDto { get; set; } = new();
    private Car FormModel { get; set; } = new();

    private bool IsLoading { get; set; } = true;
    private bool IsBusy { get; set; }
    private bool ShowForm { get; set; }
    private bool IsEditMode { get; set; }
    private string? ErrorMessage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadVehicles();
    }

    private async Task LoadVehicles()
    {
        IsLoading = true;
        ErrorMessage = null;

        try
        {
            var vehicles = await CarService.GetAllCarsAsync();
            VehicleDto = vehicles.ToList();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading vehicles: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void ShowCreate()
    {
        IsEditMode = false;
        ShowForm = true;
        FormModel = new Car
        {
            Year = DateTime.Now.Year,
            Mileage = 0,
            NumberOfDoors = 4,
            SeatingCapacity = 5,
            Status = VehicleStatus.Available,
            IsActive = true,
            FuelType = string.Empty,
            Location = string.Empty,
            BodyType = string.Empty
        };
    }

    private void EditVehicle(CarDto dto)
    {
        IsEditMode = true;
        ShowForm = true;

        // Map DTO to entity for editing
        FormModel = new Car
        {
            VehicleId = dto.VehicleId,
            VIN = dto.VIN,
            LicensePlate = dto.LicensePlate,
            Make = dto.Make,
            Model = dto.Model,
            Year = dto.Year,
            Color = dto.Color,
            Mileage = dto.Mileage,
            FuelType = dto.FuelType,
            Status = Enum.Parse<VehicleStatus>(dto.Status),
            DailyBaseRate = dto.DailyBaseRate,
            Location = dto.Location,
            ImageURL = dto.ImageURL,
            LastServiceDate = dto.LastServiceDate,
            InsuranceExpiry = dto.InsuranceExpiry,
            NumberOfDoors = dto.NumberOfDoors,
            SeatingCapacity = dto.SeatingCapacity,
            BodyType = dto.BodyType
        };
    }

    private async Task HandleValidSubmit()
    {
        IsBusy = true;
        ErrorMessage = null;

        try
        {
            if (IsEditMode)
            {
                var carDto = Mapper.Map<CarDto>(FormModel);
                await CarService.UpdateCarAsync( FormModel.VehicleId ,carDto);
            }
            else
            {
                var carDto = Mapper.Map<CarDto>(FormModel);
                await CarService.CreateCarsAsync(carDto);
            }

            ShowForm = false;
            await LoadVehicles();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error saving vehicle: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void CancelForm()
    {
        ShowForm = false;
        FormModel = new Car();
    }

    private async Task SoftDeleteVehicle(int id)
    {
        IsBusy = true;
        ErrorMessage = null;

        try
        {
            await CarService.DeleteCarAsync(id);
            await LoadVehicles();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error deleting vehicle: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static string GetStatusBadgeClass(string status)
    {
        return status switch
        {
            "Available" => "bg-success",
            "Rented" => "bg-danger",
            "Reserved" => "bg-warning text-dark",
            "Maintenance" => "bg-secondary",
            _ => "bg-dark"
        };
    }
}
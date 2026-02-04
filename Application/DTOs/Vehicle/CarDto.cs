namespace Application.DTOs.Vehicle;

public class CarDto
{
    public int VehicleId { get; set; }
    public string VIN { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Color { get; set; } = string.Empty;
    public int Mileage { get; set; }
    public string FuelType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal DailyBaseRate { get; set; }
    public string Location { get; set; } = string.Empty;
    public string? ImageURL { get; set; }
    public DateTime? LastServiceDate { get; set; }
    public DateTime? InsuranceExpiry { get; set; }
    public int NumberOfDoors { get; set; }
    public int SeatingCapacity { get; set; }
    public string BodyType { get; set; } = string.Empty;
}
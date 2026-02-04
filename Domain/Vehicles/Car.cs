using System.ComponentModel.DataAnnotations;

namespace Domain.Vehicles;

public class Car : Vehicle
{
    [Required] [Range(2, 5)] public int NumberOfDoors { get; set; }

    [Range(2, 8)] public int SeatingCapacity { get; set; } = 5;

    [StringLength(50)] public string BodyType { get; set; } = string.Empty;
    
}
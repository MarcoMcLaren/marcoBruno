namespace Domain;

public enum VehicleStatus
{
    Available = 0,      // Ready to rent
    Reserved = 1,       // Booked but not picked up
    Rented = 2,         // Currently with customer
    Maintenance = 3,    // Being serviced
    Retired = 4  
}
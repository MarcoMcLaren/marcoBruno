namespace Domain;

public enum RentalStatus
{
    Reserved = 0, // Booked but not picked up yet
    Active = 1, // Currently rented (customer has vehicle)
    Completed = 2, // Returned and finished
    Cancelled = 3 // Booking cancelled
}
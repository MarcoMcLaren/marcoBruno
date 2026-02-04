namespace Domain;

public enum UserRole
{
    Customer = 0, // Regular customer who rents vehicles
    Staff = 1, // Employee who manages rentals
    Manager = 2, // Manager with additional permissions
    Admin = 3 // System administrator
}
using Domain;
using Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Customers.AnyAsync() || await context.Vehicles.AnyAsync())
        {
            return;
        }

        await SeedCustomersAsync(context);
        await SeedVehiclesAsync(context);

        await context.SaveChangesAsync();
    }

    private static async Task SeedCustomersAsync(ApplicationDbContext context)
    {
        var customers = new List<Customer>
        {
            new Customer
            {
                FirstName = "Thabo",
                LastName = "Molefe",
                Email = "thabo.molefe@email.com",
                PhoneNumber = "082-555-0101",
                DateOfBirth = new DateTime(1985, 3, 15),
                DriverLicenseNumber = "TM8503150001",
                DriverLicenseExpiry = DateTime.Today.AddYears(5),
                DriverLicenseCountry = "RSA",
                Address = "123 Church Street",
                City = "Pretoria",
                Province = "Gauteng",
                ZipCode = "0002",
                UserRole = UserRole.Customer,
                IsActive = true
            },
            new Customer
            {
                FirstName = "Nomsa",
                LastName = "Khumalo",
                Email = "nomsa.khumalo@email.com",
                PhoneNumber = "083-555-0202",
                DateOfBirth = new DateTime(1990, 7, 22),
                DriverLicenseNumber = "NK9007220002",
                DriverLicenseExpiry = DateTime.Today.AddYears(4),
                DriverLicenseCountry = "RSA",
                Address = "456 Paul Kruger Street",
                City = "Pretoria",
                Province = "Gauteng",
                ZipCode = "0001",
                UserRole = UserRole.Customer,
                IsActive = true
            },
            new Customer
            {
                FirstName = "Johan",
                LastName = "Van Der Merwe",
                Email = "johan.vdm@email.com",
                PhoneNumber = "084-555-0303",
                DateOfBirth = new DateTime(1988, 11, 8),
                DriverLicenseNumber = "JV8811080003",
                DriverLicenseExpiry = DateTime.Today.AddYears(3),
                DriverLicenseCountry = "RSA",
                Address = "789 Pretorius Street",
                City = "Pretoria",
                Province = "Gauteng",
                ZipCode = "0083",
                UserRole = UserRole.Customer,
                IsActive = true
            },
            new Customer
            {
                FirstName = "Zanele",
                LastName = "Ndlovu",
                Email = "zanele.ndlovu@email.com",
                PhoneNumber = "072-555-0404",
                DateOfBirth = new DateTime(1992, 5, 30),
                DriverLicenseNumber = "ZN9205300004",
                DriverLicenseExpiry = DateTime.Today.AddYears(6),
                DriverLicenseCountry = "RSA",
                Address = "321 Vermeulen Street",
                City = "Pretoria",
                Province = "Gauteng",
                ZipCode = "0002",
                UserRole = UserRole.Customer,
                IsActive = true
            },
            new Customer
            {
                FirstName = "Pieter",
                LastName = "Botha",
                Email = "pieter.botha@email.com",
                PhoneNumber = "081-555-0505",
                DateOfBirth = new DateTime(1987, 9, 12),
                DriverLicenseNumber = "PB8709120005",
                DriverLicenseExpiry = DateTime.Today.AddYears(2),
                DriverLicenseCountry = "RSA",
                Address = "654 Schoeman Street",
                City = "Pretoria",
                Province = "Gauteng",
                ZipCode = "0001",
                UserRole = UserRole.Customer,
                IsActive = true
            },
            new Customer
            {
                FirstName = "Lerato",
                LastName = "Sithole",
                Email = "lerato.sithole@email.com",
                PhoneNumber = "076-555-0606",
                DateOfBirth = new DateTime(1995, 2, 18),
                DriverLicenseNumber = "LS9502180006",
                DriverLicenseExpiry = DateTime.Today.AddYears(5),
                DriverLicenseCountry = "RSA",
                Address = "147 Van Der Walt Street",
                City = "Pretoria",
                Province = "Gauteng",
                ZipCode = "0002",
                UserRole = UserRole.Customer,
                IsActive = true
            },
            new Customer
            {
                FirstName = "Michael",
                LastName = "De Wet",
                Email = "michael.dewet@email.com",
                PhoneNumber = "082-555-0707",
                DateOfBirth = new DateTime(1983, 12, 5),
                DriverLicenseNumber = "MD8312050007",
                DriverLicenseExpiry = DateTime.Today.AddYears(4),
                DriverLicenseCountry = "RSA",
                Address = "258 Andries Street",
                City = "Pretoria",
                Province = "Gauteng",
                ZipCode = "0083",
                UserRole = UserRole.Customer,
                IsActive = true
            },
            new Customer
            {
                FirstName = "Busisiwe",
                LastName = "Mthembu",
                Email = "busisiwe.mthembu@email.com",
                PhoneNumber = "073-555-0808",
                DateOfBirth = new DateTime(1991, 6, 27),
                DriverLicenseNumber = "BM9106270008",
                DriverLicenseExpiry = DateTime.Today.AddYears(3),
                DriverLicenseCountry = "RSA",
                Address = "369 Boom Street",
                City = "Pretoria",
                Province = "Gauteng",
                ZipCode = "0002",
                UserRole = UserRole.Customer,
                IsActive = true
            },
            new Customer
            {
                FirstName = "Andries",
                LastName = "Smit",
                Email = "andries.smit@email.com",
                PhoneNumber = "084-555-0909",
                DateOfBirth = new DateTime(1989, 4, 14),
                DriverLicenseNumber = "AS8904140009",
                DriverLicenseExpiry = DateTime.Today.AddYears(5),
                DriverLicenseCountry = "RSA",
                Address = "741 Struben Street",
                City = "Pretoria",
                Province = "Gauteng",
                ZipCode = "0001",
                UserRole = UserRole.Customer,
                IsActive = true
            },
            new Customer
            {
                FirstName = "Precious",
                LastName = "Zwane",
                Email = "precious.zwane@email.com",
                PhoneNumber = "071-555-1010",
                DateOfBirth = new DateTime(1994, 8, 21),
                DriverLicenseNumber = "PZ9408210010",
                DriverLicenseExpiry = DateTime.Today.AddYears(6),
                DriverLicenseCountry = "RSA",
                Address = "852 Proes Street",
                City = "Pretoria",
                Province = "Gauteng",
                ZipCode = "0083",
                UserRole = UserRole.Customer,
                IsActive = true
            }
        };

        await context.Customers.AddRangeAsync(customers);
    }

    private static async Task SeedVehiclesAsync(ApplicationDbContext context)
    {
        var vehicles = new List<Car>
        {
            new Car
            {
                VIN = "1HGBH41JXMN109186",
                LicensePlate = "CA 123 GP",
                Make = "Toyota",
                Model = "Corolla",
                Year = 2023,
                Color = "White",
                Mileage = 15000,
                FuelType = "Petrol",
                Status = VehicleStatus.Available,
                DailyBaseRate = 350.00m,
                Location = "Pretoria Central",
                NumberOfDoors = 4,
                SeatingCapacity = 5,
                BodyType = "Sedan",
                LastServiceDate = DateTime.Today.AddDays(-30),
                InsuranceExpiry = DateTime.Today.AddYears(1),
                IsActive = true
            },
            new Car
            {
                VIN = "2C3CDXHG9KH582914",
                LicensePlate = "CA 456 GP",
                Make = "Volkswagen",
                Model = "Polo Vivo",
                Year = 2022,
                Color = "Silver",
                Mileage = 28000,
                FuelType = "Petrol",
                Status = VehicleStatus.Available,
                DailyBaseRate = 300.00m,
                Location = "Pretoria Central",
                NumberOfDoors = 4,
                SeatingCapacity = 5,
                BodyType = "Hatchback",
                LastServiceDate = DateTime.Today.AddDays(-45),
                InsuranceExpiry = DateTime.Today.AddYears(1),
                IsActive = true
            },
            new Car
            {
                VIN = "3VW2B7AJ7KM045721",
                LicensePlate = "CA 789 GP",
                Make = "Hyundai",
                Model = "i20",
                Year = 2023,
                Color = "Red",
                Mileage = 12000,
                FuelType = "Petrol",
                Status = VehicleStatus.Available,
                DailyBaseRate = 320.00m,
                Location = "Pretoria Central",
                NumberOfDoors = 4,
                SeatingCapacity = 5,
                BodyType = "Hatchback",
                LastServiceDate = DateTime.Today.AddDays(-20),
                InsuranceExpiry = DateTime.Today.AddYears(1),
                IsActive = true
            },
            new Car
            {
                VIN = "5NPD84LF2KH472853",
                LicensePlate = "CA 147 GP",
                Make = "Nissan",
                Model = "Almera",
                Year = 2022,
                Color = "Blue",
                Mileage = 35000,
                FuelType = "Petrol",
                Status = VehicleStatus.Available,
                DailyBaseRate = 340.00m,
                Location = "Pretoria East",
                NumberOfDoors = 4,
                SeatingCapacity = 5,
                BodyType = "Sedan",
                LastServiceDate = DateTime.Today.AddDays(-60),
                InsuranceExpiry = DateTime.Today.AddMonths(10),
                IsActive = true
            },
            new Car
            {
                VIN = "1G1BE5SM7H7120569",
                LicensePlate = "CA 258 GP",
                Make = "Kia",
                Model = "Rio",
                Year = 2023,
                Color = "Black",
                Mileage = 8000,
                FuelType = "Petrol",
                Status = VehicleStatus.Available,
                DailyBaseRate = 330.00m,
                Location = "Pretoria East",
                NumberOfDoors = 4,
                SeatingCapacity = 5,
                BodyType = "Sedan",
                LastServiceDate = DateTime.Today.AddDays(-15),
                InsuranceExpiry = DateTime.Today.AddYears(1),
                IsActive = true
            },
            new Car
            {
                VIN = "2HGFC2F59KH541897",
                LicensePlate = "CA 369 GP",
                Make = "Honda",
                Model = "Ballade",
                Year = 2022,
                Color = "Grey",
                Mileage = 22000,
                FuelType = "Petrol",
                Status = VehicleStatus.Available,
                DailyBaseRate = 380.00m,
                Location = "Centurion",
                NumberOfDoors = 4,
                SeatingCapacity = 5,
                BodyType = "Sedan",
                LastServiceDate = DateTime.Today.AddDays(-40),
                InsuranceExpiry = DateTime.Today.AddYears(1),
                IsActive = true
            },
            new Car
            {
                VIN = "WBAJA5C55KB245876",
                LicensePlate = "CA 741 GP",
                Make = "BMW",
                Model = "320i",
                Year = 2023,
                Color = "Black",
                Mileage = 5000,
                FuelType = "Petrol",
                Status = VehicleStatus.Available,
                DailyBaseRate = 650.00m,
                Location = "Centurion",
                NumberOfDoors = 4,
                SeatingCapacity = 5,
                BodyType = "Sedan",
                LastServiceDate = DateTime.Today.AddDays(-10),
                InsuranceExpiry = DateTime.Today.AddYears(1),
                IsActive = true
            },
            new Car
            {
                VIN = "WDDGF4HB9KR258741",
                LicensePlate = "CA 852 GP",
                Make = "Mercedes-Benz",
                Model = "A-Class",
                Year = 2023,
                Color = "White",
                Mileage = 7000,
                FuelType = "Petrol",
                Status = VehicleStatus.Available,
                DailyBaseRate = 700.00m,
                Location = "Sandton",
                NumberOfDoors = 4,
                SeatingCapacity = 5,
                BodyType = "Sedan",
                LastServiceDate = DateTime.Today.AddDays(-25),
                InsuranceExpiry = DateTime.Today.AddYears(1),
                IsActive = true
            },
            new Car
            {
                VIN = "SALGS2SE5KA123456",
                LicensePlate = "CA 963 GP",
                Make = "Land Rover",
                Model = "Discovery Sport",
                Year = 2022,
                Color = "Blue",
                Mileage = 18000,
                FuelType = "Diesel",
                Status = VehicleStatus.Available,
                DailyBaseRate = 850.00m,
                Location = "Sandton",
                NumberOfDoors = 4,
                SeatingCapacity = 7,
                BodyType = "SUV",
                LastServiceDate = DateTime.Today.AddDays(-50),
                InsuranceExpiry = DateTime.Today.AddYears(1),
                IsActive = true
            },
            new Car
            {
                VIN = "MALA43AJ0K2147852",
                LicensePlate = "CA 159 GP",
                Make = "Mazda",
                Model = "CX-3",
                Year = 2023,
                Color = "Red",
                Mileage = 10000,
                FuelType = "Petrol",
                Status = VehicleStatus.Available,
                DailyBaseRate = 420.00m,
                Location = "Pretoria West",
                NumberOfDoors = 4,
                SeatingCapacity = 5,
                BodyType = "SUV",
                LastServiceDate = DateTime.Today.AddDays(-35),
                InsuranceExpiry = DateTime.Today.AddYears(1),
                IsActive = true
            }
        };

        await context.Cars.AddRangeAsync(vehicles);
    }
}
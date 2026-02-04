using Application.DTOs.Vehicle;
using AutoMapper;
using Domain.Vehicles;

namespace Application.Mappings;

public class CarProfile : Profile
{
    public CarProfile()
    {
        CreateMap<Car, CarDto>().ReverseMap();
    }
}
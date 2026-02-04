using Application.DTOs.Rental;
using AutoMapper;
using Domain;

namespace Application.Mappings;

public class RentalProfile : Profile
{
    public RentalProfile()
    {
        CreateMap<Rental, RentalDto>().ReverseMap();
    }
}
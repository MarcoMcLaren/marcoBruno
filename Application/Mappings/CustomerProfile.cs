using Application.DTOs.Customer;
using AutoMapper;
using Domain;

namespace Application.Mappings;

public sealed class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerDto>().ReverseMap();
    }
}
using AutoMapper;
using ContactManagerAPI.DTOs;
using ContactManagerBLL.Models;

namespace ContactManagerAPI;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<ContactDto, ContactModel>().ReverseMap();
    }
}
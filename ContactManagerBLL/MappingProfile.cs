using AutoMapper;
using ContactManagerBLL.Models;
using ContactManagerDAL.Entities;

namespace ContactManagerBLL;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Contact, ContactModel>();
        CreateMap<ContactModel, Contact>();
    }
}
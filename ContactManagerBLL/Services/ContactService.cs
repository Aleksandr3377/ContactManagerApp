using AutoMapper;
using ContactManagerBLL.Interfaces;
using ContactManagerBLL.Models;
using ContactManagerDAL.Entities;
using ContactManagerDAL.Interfaces;

namespace ContactManagerBLL.Services;

public class ContactService(IContactRepository contactRepository, IMapper mapper) : IContactService
{
    public async Task<IEnumerable<ContactModel>> GetAllAsync()
    {
        var entities = await contactRepository.GetAllAsync();
        return mapper.Map<IEnumerable<ContactModel>>(entities);
    }

    public async Task<ContactModel?> GetByIdAsync(int id)
    {
        var entity = await contactRepository.GetByIdAsync(id);
        return entity is null ? null : mapper.Map<ContactModel>(entity);
    }

    public async Task AddAsync(ContactModel model)
    {
        var entity = mapper.Map<Contact>(model);
        await contactRepository.AddAsync(entity);
    }

    public async Task UpdateAsync(ContactModel model)
    {
        var entity = mapper.Map<Contact>(model);
        await contactRepository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        await contactRepository.DeleteAsync(id);
    }
}
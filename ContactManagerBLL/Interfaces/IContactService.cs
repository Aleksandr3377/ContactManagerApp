using ContactManagerBLL.Models;

namespace ContactManagerBLL.Interfaces;

public interface IContactService
{
    Task<IEnumerable<ContactModel>> GetAllAsync();
    Task<ContactModel?> GetByIdAsync(int id);
    Task AddAsync(ContactModel contact);
    Task UpdateAsync(ContactModel contact);
    Task DeleteAsync(int id);
}
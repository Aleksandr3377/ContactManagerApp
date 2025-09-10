using ContactManagerBLL.Models;

namespace ContactManagerBLL.Interfaces;

public interface IContactService
{
    Task<IEnumerable<ContactModel>> GetAllAsync();
    Task<ContactModel?> GetByIdAsync(int id);
    Task AddAsync(ContactModel contact);
    Task<(int imported, List<string> errors)> ImportCsvAsync(Stream csvStream, CancellationToken ct = default);
    Task UpdateAsync(ContactModel contact);
    Task DeleteAsync(int id);
}
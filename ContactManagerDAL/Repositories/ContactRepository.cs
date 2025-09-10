using ContactManagerDAL.Entities;
using ContactManagerDAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerDAL.Repositories;

public class ContactRepository(ContactDbContext context) : IContactRepository
{
    public async Task<IEnumerable<Contact>> GetAllAsync()
        => await context.Contacts.AsNoTracking().ToListAsync();

    public async Task<Contact?> GetByIdAsync(int id)
        => await context.Contacts.FindAsync(id);

    public async Task AddAsync(Contact contact)
    {
        context.Contacts.Add(contact);
        await context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<Contact> contacts)
    {
        context.Contacts.AddRange(contacts);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Contact contact)
    {
        context.Contacts.Update(contact);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await context.Contacts.FindAsync(id);
        if (entity != null)
        {
            context.Contacts.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
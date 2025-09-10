using ContactManagerDAL.Entities;

namespace ContactManagerDAL;

public static class SeedData
{
    public static void Initialize(ContactDbContext context)
    {
        if (context.Contacts.Any()) return;

        context.Contacts.AddRange(
        new Contact
        {
            Name = "Alex Johnson",
            DateOfBirth = new DateTime(1990, 5, 12),
            Married = true,
            Phone = "+1234567890",
            Salary = 50000
        },
        new Contact
        {
            Name = "Maria Smith",
            DateOfBirth = new DateTime(1985, 11, 23),
            Married = false,
            Phone = "+9876543210",
            Salary = 70000
        }
        );

        context.SaveChanges();
    }
}
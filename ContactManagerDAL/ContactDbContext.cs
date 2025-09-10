using ContactManagerDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerDAL;

public class ContactDbContext(DbContextOptions<ContactDbContext> options) : DbContext(options)
{
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Contact>(entity => {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.DateOfBirth)
                .IsRequired();

            entity.Property(e => e.Married)
                .IsRequired();

            entity.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Salary)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        });
    }
}
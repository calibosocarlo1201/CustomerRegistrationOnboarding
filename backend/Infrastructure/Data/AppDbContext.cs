using CustomerRegistrationOnboarding.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerRegistrationOnboarding.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(c => c.LastName).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(c => c.Email).IsUnique();
            entity.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(20);
            entity.Property(c => c.SignatureBase64);
            entity.Property(c => c.DateCreated).IsRequired();
        });
    }
}

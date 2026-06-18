using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.FirstName).HasMaxLength(100).IsRequired(true);
            builder.Property(c => c.LastName).HasMaxLength(100).IsRequired(true);
            builder.Property(c => c.Email).HasMaxLength(256).IsRequired(true);
            builder.HasIndex(c => c.Email).IsUnique();
            builder.Property(c => c.CreatedAt).IsRequired(true);
            builder.HasMany(c => c.Orders)
                    .WithOne(o => o.Customer)
                    .HasForeignKey(o => o.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Property(c => c.PasswordHash).IsRequired().HasMaxLength(500);
            builder.ToTable("Customers");

        }
    }
}

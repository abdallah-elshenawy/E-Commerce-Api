using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace E_Commerce.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasMaxLength(200).IsRequired(true);
            builder.Property(p => p.Description).HasMaxLength(1000).IsRequired(false);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.Property(p => p.StockQuantity).IsRequired(true);
            builder.ToTable("Products");

            builder.Property(p => p.RowVersion).IsRowVersion();
            builder.Property(p => p.CategoryId).IsRequired(true);
            builder.HasIndex(p => p.Price);
            builder.HasIndex(p => p.Name).IsUnique(true);
            builder.HasIndex(p => p.StockQuantity);
        }
    }
}

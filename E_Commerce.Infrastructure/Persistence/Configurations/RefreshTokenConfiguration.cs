
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.Id);
            builder.Property(rt => rt.Token).HasMaxLength(500).IsRequired(true);
            builder.HasIndex(rt => rt.Token).IsUnique();
            builder.HasOne(rt => rt.Customer)
                .WithMany(c => c.RefreshTokens)
                .HasForeignKey(rt => rt.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.ToTable("RefreshTokens");
        }
    }
}

using BiddingManagementSystem.Domain.Aggregates.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BiddingManagementSystem.Infrastructure.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");
            
            builder.HasKey(t => t.Id);
            
            builder.Property(t => t.Token)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(t => t.ExpiryDate)
                .IsRequired();
                
            builder.Property(t => t.IsRevoked)
                .IsRequired();
        }
    }
}

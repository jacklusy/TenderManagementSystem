using BiddingManagementSystem.Domain.Aggregates.UserAggregate;
using BiddingManagementSystem.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BiddingManagementSystem.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            
            builder.HasKey(u => u.Id);
            
            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(u => u.PasswordHash)
                .IsRequired();
                
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(20);
                
            builder.Property(u => u.Role)
                .IsRequired();
                
            builder.Property(u => u.IsActive)
                .IsRequired();
                
            // Value Object mapping
            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Value)
                    .HasColumnName("Email")
                    .IsRequired()
                    .HasMaxLength(100);
                    
                email.HasIndex(e => e.Value)
                    .IsUnique();
            });
            
            builder.OwnsOne(u => u.CompanyAddress, address =>
            {
                address.Property(a => a.Street)
                    .HasColumnName("CompanyStreet")
                    .HasMaxLength(200);
                    
                address.Property(a => a.City)
                    .HasColumnName("CompanyCity")
                    .HasMaxLength(100);
                    
                address.Property(a => a.State)
                    .HasColumnName("CompanyState")
                    .HasMaxLength(100);
                    
                address.Property(a => a.Country)
                    .HasColumnName("CompanyCountry")
                    .HasMaxLength(100);
                    
                address.Property(a => a.ZipCode)
                    .HasColumnName("CompanyZipCode")
                    .HasMaxLength(20);
            });
            
            builder.Property(u => u.CompanyName)
                .HasMaxLength(200);
                
            builder.Property(u => u.RegistrationNumber)
                .HasMaxLength(50);
                
            // Relationship with RefreshToken
            builder.HasMany(u => u.RefreshTokens)
                .WithOne()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade);
                
            // Indices
            builder.HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}

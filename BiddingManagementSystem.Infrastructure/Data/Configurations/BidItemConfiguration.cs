using BiddingManagementSystem.Domain.Aggregates.BidAggregate;
using BiddingManagementSystem.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BiddingManagementSystem.Infrastructure.Data.Configurations
{
    public class BidItemConfiguration : IEntityTypeConfiguration<BidItem>
    {
        public void Configure(EntityTypeBuilder<BidItem> builder)
        {
            builder.ToTable("BidItems");
            
            builder.HasKey(i => i.Id);
            
            builder.Property(i => i.Description)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(i => i.Quantity)
                .IsRequired();
                
            // Value Object mapping
            builder.OwnsOne(i => i.UnitPrice, price =>
            {
                price.Property(m => m.Amount)
                    .HasColumnName("UnitPrice")
                    .HasColumnType("decimal(18, 2)")
                    .IsRequired();
                    
                price.Property(m => m.Currency)
                    .HasColumnName("UnitPriceCurrency")
                    .IsRequired()
                    .HasMaxLength(3);
            });
            
            builder.OwnsOne(i => i.TotalPrice, price =>
            {
                price.Property(m => m.Amount)
                    .HasColumnName("TotalPrice")
                    .HasColumnType("decimal(18, 2)")
                    .IsRequired();
                    
                price.Property(m => m.Currency)
                    .HasColumnName("TotalPriceCurrency")
                    .IsRequired()
                    .HasMaxLength(3);
            });
        }
    }
}
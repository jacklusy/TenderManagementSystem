using BiddingManagementSystem.Domain.Aggregates.BidAggregate;
using BiddingManagementSystem.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BiddingManagementSystem.Infrastructure.Data.Configurations
{
    public class BidConfiguration : IEntityTypeConfiguration<Bid>
    {
        public void Configure(EntityTypeBuilder<Bid> builder)
        {
            builder.ToTable("Bids");
            
            builder.HasKey(b => b.Id);
            
            builder.Property(b => b.TenderId)
                .IsRequired();
                
            builder.Property(b => b.BidderId)
                .IsRequired();
                
            builder.Property(b => b.Status)
                .IsRequired();
                
            builder.Property(b => b.TechnicalProposalSummary)
                .IsRequired()
                .HasMaxLength(2000);
                
            builder.Property(b => b.Score)
                .HasColumnType("decimal(5, 2)");
                
            builder.Property(b => b.EvaluationComments)
                .HasMaxLength(1000);
                
            // Value Object mapping
            builder.OwnsOne(b => b.BidAmount, amount =>
            {
                amount.Property(m => m.Amount)
                    .HasColumnName("BidAmount")
                    .HasColumnType("decimal(18, 2)")
                    .IsRequired();
                    
                amount.Property(m => m.Currency)
                    .HasColumnName("BidCurrency")
                    .IsRequired()
                    .HasMaxLength(3);
            });
            
            // Relationships
            builder.HasMany(b => b.BidItems)
                .WithOne()
                .HasForeignKey("BidId")
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(b => b.Documents)
                .WithOne()
                .HasForeignKey("BidId")
                .OnDelete(DeleteBehavior.Cascade);
                
            // Indices
            builder.HasIndex(b => b.TenderId);
            builder.HasIndex(b => b.BidderId);
            builder.HasIndex(b => b.Status);
            
            builder.HasIndex(b => new { b.TenderId, b.BidderId })
                .IsUnique();
        }
    }
}
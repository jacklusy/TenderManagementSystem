using BiddingManagementSystem.Domain.Aggregates.TenderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BiddingManagementSystem.Infrastructure.Data.Configurations
{
    public class TenderPaymentTermConfiguration : IEntityTypeConfiguration<TenderPaymentTerm>
    {
        public void Configure(EntityTypeBuilder<TenderPaymentTerm> builder)
        {
            builder.ToTable("TenderPaymentTerms");
            
            builder.HasKey(p => p.Id);
            
            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(p => p.Percentage)
                .IsRequired()
                .HasColumnType("decimal(5, 2)");
        }
    }
}
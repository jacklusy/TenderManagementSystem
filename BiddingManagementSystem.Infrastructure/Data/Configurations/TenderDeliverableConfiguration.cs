using BiddingManagementSystem.Domain.Aggregates.TenderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BiddingManagementSystem.Infrastructure.Data.Configurations
{
    public class TenderDeliverableConfiguration : IEntityTypeConfiguration<TenderDeliverable>
    {
        public void Configure(EntityTypeBuilder<TenderDeliverable> builder)
        {
            builder.ToTable("TenderDeliverables");
            
            builder.HasKey(d => d.Id);
            
            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(d => d.Description)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}
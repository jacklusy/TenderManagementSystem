using BiddingManagementSystem.Domain.Aggregates.TenderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BiddingManagementSystem.Infrastructure.Data.Configurations
{
    public class EligibilityCriteriaConfiguration : IEntityTypeConfiguration<EligibilityCriteria>
    {
        public void Configure(EntityTypeBuilder<EligibilityCriteria> builder)
        {
            builder.ToTable("EligibilityCriteria");
            
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.CriteriaName)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(e => e.CriteriaDescription)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}
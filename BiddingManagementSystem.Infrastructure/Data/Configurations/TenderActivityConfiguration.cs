using BiddingManagementSystem.Domain.Aggregates.TenderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BiddingManagementSystem.Infrastructure.Data.Configurations
{
    public class TenderActivityConfiguration : IEntityTypeConfiguration<TenderActivity>
    {
        public void Configure(EntityTypeBuilder<TenderActivity> builder)
        {
            builder.ToTable("TenderActivities");
            
            builder.HasKey(a => a.Id);
            
            builder.Property(a => a.ActivityName)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(a => a.ExpectedDate)
                .IsRequired();
        }
    }
}

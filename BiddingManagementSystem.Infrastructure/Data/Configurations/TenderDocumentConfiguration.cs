using BiddingManagementSystem.Domain.Aggregates.TenderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BiddingManagementSystem.Infrastructure.Data.Configurations
{
    public class TenderDocumentConfiguration : IEntityTypeConfiguration<TenderDocument>
    {
        public void Configure(EntityTypeBuilder<TenderDocument> builder)
        {
            builder.ToTable("TenderDocuments");
            
            builder.HasKey(d => d.Id);
            
            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(d => d.FileUrl)
                .IsRequired()
                .HasMaxLength(500);
                
            builder.Property(d => d.Description)
                .HasMaxLength(500);
                
            builder.Property(d => d.FileType)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(d => d.FileSize)
                .IsRequired();
        }
    }
}
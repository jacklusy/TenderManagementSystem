using BiddingManagementSystem.Domain.Aggregates.BidAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BiddingManagementSystem.Infrastructure.Data.Configurations
{
    public class BidDocumentConfiguration : IEntityTypeConfiguration<BidDocument>
    {
        public void Configure(EntityTypeBuilder<BidDocument> builder)
        {
            builder.ToTable("BidDocuments");
            
            builder.HasKey(d => d.Id);
            
            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(d => d.FileUrl)
                .IsRequired()
                .HasMaxLength(500);
                
            builder.Property(d => d.DocumentType)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(d => d.FileType)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(d => d.FileSize)
                .IsRequired();
        }
    }
}
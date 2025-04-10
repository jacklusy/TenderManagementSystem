using BiddingManagementSystem.Domain.Aggregates.TenderAggregate;
using BiddingManagementSystem.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BiddingManagementSystem.Infrastructure.Data.Configurations
{
    public class TenderConfiguration : IEntityTypeConfiguration<Tender>
    {
        public void Configure(EntityTypeBuilder<Tender> builder)
        {
            builder.ToTable("Tenders");
            
            builder.HasKey(t => t.Id);
            
            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(t => t.ReferenceNumber)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(2000);
                
            builder.Property(t => t.IssueDate)
                .IsRequired();
                
            builder.Property(t => t.ClosingDate)
                .IsRequired();
                
            builder.Property(t => t.Type)
                .IsRequired();
                
            builder.Property(t => t.Category)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(t => t.ContactEmail)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(t => t.IssuedByOrganization)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(t => t.Status)
                .IsRequired();
                
            // Value Object mapping
            builder.OwnsOne(t => t.BudgetRange, budget =>
            {
                budget.Property(m => m.Amount)
                    .HasColumnName("BudgetAmount")
                    .HasColumnType("decimal(18, 2)")
                    .IsRequired();
                    
                budget.Property(m => m.Currency)
                    .HasColumnName("BudgetCurrency")
                    .IsRequired()
                    .HasMaxLength(3);
            });
            
            // Relationships
            builder.HasMany(t => t.Documents)
                .WithOne()
                .HasForeignKey("TenderId")
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(t => t.EligibilityCriteria)
                .WithOne()
                .HasForeignKey("TenderId")
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(t => t.Deliverables)
                .WithOne()
                .HasForeignKey("TenderId")
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(t => t.Timeline)
                .WithOne()
                .HasForeignKey("TenderId")
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(t => t.PaymentTerms)
                .WithOne()
                .HasForeignKey("TenderId")
                .OnDelete(DeleteBehavior.Cascade);
                
            // Indices
            builder.HasIndex(t => t.ReferenceNumber)
                .IsUnique();
                
            builder.HasIndex(t => t.Status);
            builder.HasIndex(t => t.Type);
            builder.HasIndex(t => t.Category);
            builder.HasIndex(t => t.ClosingDate);
        }
    }
}
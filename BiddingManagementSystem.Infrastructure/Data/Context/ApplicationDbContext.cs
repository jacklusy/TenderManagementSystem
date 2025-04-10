using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BiddingManagementSystem.Domain.Aggregates.BidAggregate;
using BiddingManagementSystem.Domain.Aggregates.TenderAggregate;
using BiddingManagementSystem.Domain.Aggregates.UserAggregate;
using BiddingManagementSystem.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BiddingManagementSystem.Infrastructure.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public DbSet<User> Users { get; set; }
        public DbSet<Tender> Tenders { get; set; }
        public DbSet<Bid> Bids { get; set; }
        
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options, 
            IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Load all entity configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            // Get current user id from claims
            var userId = _httpContextAccessor?.HttpContext?.User?.FindFirst("sub")?.Value;
            
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.SetCreatedBy(userId ?? "System");
                        break;
                    case EntityState.Modified:
                        entry.Entity.SetUpdatedBy(userId ?? "System");
                        break;
                }
            }
            
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
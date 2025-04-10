using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiddingManagementSystem.Domain.Aggregates.TenderAggregate;
using BiddingManagementSystem.Domain.Enums;
using BiddingManagementSystem.Domain.Repositories;
using BiddingManagementSystem.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BiddingManagementSystem.Infrastructure.Data.Repositories
{
    public class TenderRepository : ITenderRepository
    {
        private readonly ApplicationDbContext _context;

        public TenderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Tender> GetByIdAsync(Guid id)
        {
            return await _context.Tenders
                .Include(t => t.Documents)
                .Include(t => t.EligibilityCriteria)
                .Include(t => t.Deliverables)
                .Include(t => t.Timeline)
                .Include(t => t.PaymentTerms)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tender> GetByReferenceNumberAsync(string referenceNumber)
        {
            return await _context.Tenders
                .Include(t => t.Documents)
                .Include(t => t.EligibilityCriteria)
                .Include(t => t.Deliverables)
                .Include(t => t.Timeline)
                .Include(t => t.PaymentTerms)
                .FirstOrDefaultAsync(t => t.ReferenceNumber == referenceNumber);
        }

        public async Task<IEnumerable<Tender>> GetAllAsync()
        {
            return await _context.Tenders
                .Include(t => t.Documents)
                .Include(t => t.EligibilityCriteria)
                .Include(t => t.Deliverables)
                .Include(t => t.Timeline)
                .Include(t => t.PaymentTerms)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tender>> GetByStatusAsync(TenderStatus status)
        {
            return await _context.Tenders
                .Include(t => t.Documents)
                .Include(t => t.EligibilityCriteria)
                .Where(t => t.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tender>> GetByTypeAsync(TenderType type)
        {
            return await _context.Tenders
                .Include(t => t.Documents)
                .Include(t => t.EligibilityCriteria)
                .Where(t => t.Type == type)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tender>> GetByCategoryAsync(string category)
        {
            return await _context.Tenders
                .Include(t => t.Documents)
                .Include(t => t.EligibilityCriteria)
                .Where(t => t.Category == category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tender>> GetClosingSoonAsync(int daysThreshold)
        {
            var thresholdDate = DateTime.UtcNow.AddDays(daysThreshold);
            return await _context.Tenders
                .Include(t => t.Documents)
                .Include(t => t.EligibilityCriteria)
                .Where(t => t.Status == TenderStatus.Published && 
                           t.ClosingDate <= thresholdDate)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Tenders.AnyAsync(t => t.Id == id);
        }

        public async Task<bool> ExistsByReferenceNumberAsync(string referenceNumber)
        {
            return await _context.Tenders.AnyAsync(t => t.ReferenceNumber == referenceNumber);
        }

        public async Task AddAsync(Tender tender)
        {
            await _context.Tenders.AddAsync(tender);
        }

        public Task UpdateAsync(Tender tender)
        {
            _context.Tenders.Update(tender);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Tender tender)
        {
            _context.Tenders.Remove(tender);
            return Task.CompletedTask;
        }
    }
}
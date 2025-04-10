using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiddingManagementSystem.Domain.Aggregates.BidAggregate;
using BiddingManagementSystem.Domain.Enums;
using BiddingManagementSystem.Domain.Repositories;
using BiddingManagementSystem.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BiddingManagementSystem.Infrastructure.Data.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly ApplicationDbContext _context;

        public BidRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Bid> GetByIdAsync(Guid id)
        {
            return await _context.Bids
                .Include(b => b.BidItems)
                .Include(b => b.Documents)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Bid>> GetAllAsync()
        {
            return await _context.Bids
                .Include(b => b.BidItems)
                .Include(b => b.Documents)
                .ToListAsync();
        }

        public async Task<IEnumerable<Bid>> GetByTenderIdAsync(Guid tenderId)
        {
            return await _context.Bids
                .Include(b => b.BidItems)
                .Include(b => b.Documents)
                .Where(b => b.TenderId == tenderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Bid>> GetByBidderIdAsync(Guid bidderId)
        {
            return await _context.Bids
                .Include(b => b.BidItems)
                .Include(b => b.Documents)
                .Where(b => b.BidderId == bidderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Bid>> GetByStatusAsync(BidStatus status)
        {
            return await _context.Bids
                .Include(b => b.BidItems)
                .Include(b => b.Documents)
                .Where(b => b.Status == status)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Bids.AnyAsync(b => b.Id == id);
        }

        public async Task<bool> ExistsByTenderAndBidderAsync(Guid tenderId, Guid bidderId)
        {
            return await _context.Bids.AnyAsync(b => b.TenderId == tenderId && b.BidderId == bidderId);
        }

        public async Task AddAsync(Bid bid)
        {
            await _context.Bids.AddAsync(bid);
        }

        public Task UpdateAsync(Bid bid)
        {
            _context.Bids.Update(bid);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Bid bid)
        {
            _context.Bids.Remove(bid);
            return Task.CompletedTask;
        }
    }
}
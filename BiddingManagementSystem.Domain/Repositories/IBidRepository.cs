using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BiddingManagementSystem.Domain.Aggregates.BidAggregate;
using BiddingManagementSystem.Domain.Enums;

namespace BiddingManagementSystem.Domain.Repositories
{
    public interface IBidRepository
    {
        Task<Bid> GetByIdAsync(Guid id);
        Task<IEnumerable<Bid>> GetAllAsync();
        Task<IEnumerable<Bid>> GetByTenderIdAsync(Guid tenderId);
        Task<IEnumerable<Bid>> GetByBidderIdAsync(Guid bidderId);
        Task<IEnumerable<Bid>> GetByStatusAsync(BidStatus status);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> ExistsByTenderAndBidderAsync(Guid tenderId, Guid bidderId);
        Task AddAsync(Bid bid);
        Task UpdateAsync(Bid bid);
        Task DeleteAsync(Bid bid);
    }
}
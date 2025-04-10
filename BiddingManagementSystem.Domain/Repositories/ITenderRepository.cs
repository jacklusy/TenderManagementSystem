using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BiddingManagementSystem.Domain.Aggregates.TenderAggregate;
using BiddingManagementSystem.Domain.Enums;

namespace BiddingManagementSystem.Domain.Repositories
{
    public interface ITenderRepository
    {
        Task<Tender> GetByIdAsync(Guid id);
        Task<Tender> GetByReferenceNumberAsync(string referenceNumber);
        Task<IEnumerable<Tender>> GetAllAsync();
        Task<IEnumerable<Tender>> GetByStatusAsync(TenderStatus status);
        Task<IEnumerable<Tender>> GetByTypeAsync(TenderType type);
        Task<IEnumerable<Tender>> GetByCategoryAsync(string category);
        Task<IEnumerable<Tender>> GetClosingSoonAsync(int daysThreshold);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> ExistsByReferenceNumberAsync(string referenceNumber);
        Task AddAsync(Tender tender);
        Task UpdateAsync(Tender tender);
        Task DeleteAsync(Tender tender);
    }
}
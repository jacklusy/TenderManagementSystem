using System;
using System.Threading.Tasks;

namespace BiddingManagementSystem.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ITenderRepository Tenders { get; }
        IBidRepository Bids { get; }
        
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<int> SaveChangesAsync();
    }
}
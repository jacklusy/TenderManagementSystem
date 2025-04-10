using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BiddingManagementSystem.Domain.Aggregates.UserAggregate;
using BiddingManagementSystem.Domain.ValueObjects;

namespace BiddingManagementSystem.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(Email email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<bool> ExistsAsync(Guid id);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<bool> ExistsByEmailAsync(Email email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
    }
}
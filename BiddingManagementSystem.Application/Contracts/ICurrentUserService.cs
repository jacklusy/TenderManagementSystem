using BiddingManagementSystem.Domain.Enums;

namespace BiddingManagementSystem.Application.Contracts
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string Username { get; }
        UserRole? Role { get; }
        bool IsAuthenticated { get; }
    }
} 
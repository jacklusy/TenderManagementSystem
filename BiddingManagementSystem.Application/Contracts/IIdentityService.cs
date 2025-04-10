using System.Threading.Tasks;
using BiddingManagementSystem.Application.DTOs.Auth;

namespace BiddingManagementSystem.Application.Contracts
{
    public interface IIdentityService
    {
        Task<AuthResponseDto> RegisterUserAsync(RegisterUserDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
        Task RevokeTokenAsync(string token);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<bool> ValidateUserAsync(string username, string password);
    }
} 
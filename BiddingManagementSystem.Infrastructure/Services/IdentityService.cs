using System;
using System.Threading.Tasks;
using AutoMapper;
using BiddingManagementSystem.Application.Contracts;
using BiddingManagementSystem.Application.DTOs.Auth;
using BiddingManagementSystem.Domain.Aggregates.UserAggregate;
using BiddingManagementSystem.Domain.Repositories;
using BiddingManagementSystem.Domain.ValueObjects;
using BiddingManagementSystem.Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BiddingManagementSystem.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;
        private readonly ILogger<IdentityService> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;

        public IdentityService(
            IUnitOfWork unitOfWork, 
            IJwtTokenGenerator jwtTokenGenerator, 
            IMapper mapper, 
            ILogger<IdentityService> logger,
            IPasswordHasher<User> passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthResponseDto> RegisterUserAsync(RegisterUserDto registerDto)
        {
            // Check if username already exists
            if (await _unitOfWork.Users.ExistsByUsernameAsync(registerDto.Username))
            {
                throw new ArgumentException("Username already exists");
            }

            // Check if email already exists
            var email = new Email(registerDto.Email);
            if (await _unitOfWork.Users.ExistsByEmailAsync(email))
            {
                throw new ArgumentException("Email is already registered");
            }

            // Create user entity
            var user = _mapper.Map<User>(registerDto);
            
            // Hash the password
            var passwordHash = _passwordHasher.HashPassword(user, registerDto.Password);
            user.UpdatePasswordHash(passwordHash);

            // If role is Bidder, add company details
            if (user.Role == Domain.Enums.UserRole.Bidder && registerDto.CompanyName != null)
            {
                var address = _mapper.Map<Address>(registerDto.CompanyAddress);
                user.SetCompanyDetails(registerDto.CompanyName, registerDto.RegistrationNumber, address);
            }

            // Generate JWT token
            var accessToken = _jwtTokenGenerator.GenerateAccessToken(user);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();
            var refreshTokenExpiryDate = _jwtTokenGenerator.GetRefreshTokenExpiryDate();

            // Add refresh token to user before saving to database
            user.AddRefreshToken(refreshToken, refreshTokenExpiryDate);

            // Add user to repository and save all changes in one transaction
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error registering user {Username}", registerDto.Username);
                throw;
            }

            // Return auth response
            var response = _mapper.Map<AuthResponseDto>(user);
            response.AccessToken = accessToken;
            response.RefreshToken = refreshToken;

            return response;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _unitOfWork.Users.GetByUsernameAsync(loginDto.Username);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            // Verify password
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(
                user, user.PasswordHash, loginDto.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            // Check if user is active
            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("User account is deactivated");
            }

            // Record login
            user.RecordLogin();

            // Generate JWT token
            var accessToken = _jwtTokenGenerator.GenerateAccessToken(user);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();
            var refreshTokenExpiryDate = _jwtTokenGenerator.GetRefreshTokenExpiryDate();

            // Add refresh token to user
            user.AddRefreshToken(refreshToken, refreshTokenExpiryDate);
            
            // Save all changes in a transaction
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error logging in user {Username}", loginDto.Username);
                throw;
            }

            // Return auth response
            var response = _mapper.Map<AuthResponseDto>(user);
            response.AccessToken = accessToken;
            response.RefreshToken = refreshToken;

            return response;
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            // Validate refresh token
            var user = await ValidateRefreshTokenAsync(refreshTokenDto.RefreshToken);

            // Revoke the used refresh token
            user.RevokeRefreshToken(refreshTokenDto.RefreshToken);

            // Generate new tokens
            var accessToken = _jwtTokenGenerator.GenerateAccessToken(user);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();
            var refreshTokenExpiryDate = _jwtTokenGenerator.GetRefreshTokenExpiryDate();

            // Add new refresh token to user
            user.AddRefreshToken(refreshToken, refreshTokenExpiryDate);
            
            // Save all changes in a transaction
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error refreshing token for user {UserId}", user.Id);
                throw;
            }

            // Return auth response
            var response = _mapper.Map<AuthResponseDto>(user);
            response.AccessToken = accessToken;
            response.RefreshToken = refreshToken;

            return response;
        }

        public async Task RevokeTokenAsync(string token)
        {
            var user = await ValidateRefreshTokenAsync(token);
            user.RevokeRefreshToken(token);
            
            // Save changes in a transaction
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error revoking token for user {UserId}", user.Id);
                throw;
            }
        }

        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(Guid.Parse(userId));
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            // Verify current password
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(
                user, user.PasswordHash, currentPassword);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return false;
            }

            // Update password
            var newPasswordHash = _passwordHasher.HashPassword(user, newPassword);
            user.UpdatePasswordHash(newPasswordHash);
            
            // Save changes in a transaction
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error changing password for user {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            var user = await _unitOfWork.Users.GetByUsernameAsync(username);
            if (user == null || !user.IsActive)
            {
                return false;
            }

            // Verify password
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(
                user, user.PasswordHash, password);

            return passwordVerificationResult != PasswordVerificationResult.Failed;
        }

        private async Task<User> ValidateRefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentException("Invalid refresh token");
            }

            var users = await _unitOfWork.Users.GetAllAsync();
            var user = users.FirstOrDefault(u => u.HasValidRefreshToken(refreshToken));

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            return user;
        }
    }
} 
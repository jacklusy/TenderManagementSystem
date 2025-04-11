using System;
using System.Threading.Tasks;
using AutoMapper;
using BiddingManagementSystem.Application.Contracts;
using BiddingManagementSystem.Application.DTOs.Auth;
using BiddingManagementSystem.Domain.Aggregates.UserAggregate;
using BiddingManagementSystem.Domain.Repositories;
using BiddingManagementSystem.Domain.ValueObjects;
using BiddingManagementSystem.Domain.Exceptions;
using BiddingManagementSystem.Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Linq;

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
            try
            {
                // Check if username already exists
                if (await _unitOfWork.Users.ExistsByUsernameAsync(registerDto.Username))
                {
                    throw new ArgumentException("Username already exists");
                }

                // Create and validate email
                Email email;
                try
                {
                    email = new Email(registerDto.Email);
                }
                catch (InvalidEmailException ex)
                {
                    _logger.LogWarning("Invalid email during registration: {Email}, Error: {Error}", 
                        registerDto.Email, ex.Message);
                    throw new ArgumentException(ex.Message, ex);
                }

                // Check if email already exists
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
                    try
                    {
                        var address = _mapper.Map<Address>(registerDto.CompanyAddress);
                        user.SetCompanyDetails(registerDto.CompanyName, registerDto.RegistrationNumber, address);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning("Error setting company details: {Error}", ex.Message);
                        throw new ArgumentException("Invalid company details: " + ex.Message, ex);
                    }
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
                    throw new InvalidOperationException("Failed to create user account: " + ex.Message, ex);
                }

                // Return auth response
                var response = _mapper.Map<AuthResponseDto>(user);
                response.AccessToken = accessToken;
                response.RefreshToken = refreshToken;

                return response;
            }
            catch (ArgumentException)
            {
                // Rethrow argument exceptions directly as these are validation errors
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during user registration");
                throw new InvalidOperationException("An unexpected error occurred during registration.", ex);
            }
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                _logger.LogInformation("Login attempt for user: {Username}", loginDto.Username);
                
                var user = await _unitOfWork.Users.GetByUsernameAsync(loginDto.Username);
                
                // Fixed logging format
                _logger.LogDebug("User found: {UserFound}", user != null);
                
                if (user == null)
                {
                    _logger.LogWarning("Login failed - User not found: {Username}", loginDto.Username);
                    throw new UnauthorizedAccessException("Invalid username or password");
                }

                // Verify password
                _logger.LogDebug("Verifying password for user: {Username}", loginDto.Username);
                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(
                    user, user.PasswordHash, loginDto.Password);

                if (passwordVerificationResult == PasswordVerificationResult.Failed)
                {
                    _logger.LogWarning("Login failed - Invalid password for user: {Username}", loginDto.Username);
                    throw new UnauthorizedAccessException("Invalid username or password");
                }

                // Check if user is active
                if (!user.IsActive)
                {
                    _logger.LogWarning("Login failed - User account is deactivated: {Username}", loginDto.Username);
                    throw new UnauthorizedAccessException("User account is deactivated");
                }

                // Generate JWT token first
                _logger.LogDebug("Generating access token for user: {Username}", loginDto.Username);
                var accessToken = _jwtTokenGenerator.GenerateAccessToken(user);
                _logger.LogDebug("Generating refresh token for user: {Username}", loginDto.Username);
                var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();
                var refreshTokenExpiryDate = _jwtTokenGenerator.GetRefreshTokenExpiryDate();

                // Create a response immediately
                var response = _mapper.Map<AuthResponseDto>(user);
                response.AccessToken = accessToken;
                response.RefreshToken = refreshToken;
                
                // Record login and add the refresh token in a separate operation
                // This is fire-and-forget to avoid blocking the login response
                try
                {
                    // First record login time
                    user.RecordLogin();
                    _logger.LogDebug("Login recorded for user: {Username}", loginDto.Username);
                    
                    // Add refresh token to user and save
                    try
                    {
                        user.AddRefreshToken(refreshToken, refreshTokenExpiryDate);
                        _logger.LogDebug("Refresh token added to user: {Username}", loginDto.Username);
                        
                        await _unitOfWork.SaveChangesAsync();
                        _logger.LogDebug("User data saved successfully");
                    }
                    catch (Exception ex)
                    {
                        // Just log the error but don't block login
                        _logger.LogError(ex, "Error saving refresh token for user: {Username}", loginDto.Username);
                        // We can still proceed with login even if refresh token saving fails
                    }
                }
                catch (Exception ex)
                {
                    // Just log the error but don't block login
                    _logger.LogError(ex, "Error updating login information for user: {Username}", loginDto.Username);
                }

                _logger.LogInformation("Login successful for user: {Username}", loginDto.Username);
                return response;
            }
            catch (UnauthorizedAccessException)
            {
                // Rethrow authentication errors directly
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login");
                throw new InvalidOperationException("An unexpected error occurred during login", ex);
            }
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
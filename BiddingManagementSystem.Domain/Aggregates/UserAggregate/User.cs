using System;
using System.Collections.Generic;
using BiddingManagementSystem.Domain.Common;
using BiddingManagementSystem.Domain.Enums;
using BiddingManagementSystem.Domain.ValueObjects;

namespace BiddingManagementSystem.Domain.Aggregates.UserAggregate
{
    public class User : BaseEntity, IAggregateRoot
    {
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Email Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public UserRole Role { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime? LastLoginAt { get; private set; }
        
        // For bidders/companies
        public string CompanyName { get; private set; }
        public string RegistrationNumber { get; private set; }
        public Address CompanyAddress { get; private set; }
        
        private readonly List<RefreshToken> _refreshTokens = new();
        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();
        
        private User() { }
        
        public User(
            string username,
            string passwordHash,
            string firstName,
            string lastName,
            Email email,
            string phoneNumber,
            UserRole role)
        {
            Username = username;
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Role = role;
            IsActive = true;
        }
        
        public void SetCompanyDetails(string companyName, string registrationNumber, Address companyAddress)
        {
            if (Role != UserRole.Bidder)
                throw new InvalidOperationException("Only bidders can have company details");
                
            CompanyName = companyName;
            RegistrationNumber = registrationNumber;
            CompanyAddress = companyAddress;
        }
        
        public void UpdatePasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
            SetUpdatedBy(Id.ToString());
        }
        
        public void Deactivate()
        {
            IsActive = false;
            SetUpdatedBy(Id.ToString());
        }
        
        public void Activate()
        {
            IsActive = true;
            SetUpdatedBy(Id.ToString());
        }
        
        public void RecordLogin()
        {
            LastLoginAt = DateTime.UtcNow;
        }
        
        public void AddRefreshToken(string token, DateTime expiryDate)
        {
            var refreshToken = new RefreshToken(token, expiryDate);
            
            // Set the CreatedBy and UpdatedBy fields to avoid NULL values
            refreshToken.SetCreatedBy(Id.ToString());
            refreshToken.SetUpdatedBy(Id.ToString());
            
            _refreshTokens.Add(refreshToken);
            
            // Remove expired tokens
            _refreshTokens.RemoveAll(t => t.ExpiryDate < DateTime.UtcNow);
            
            // Limit number of refresh tokens per user
            if (_refreshTokens.Count > 5)
                _refreshTokens.RemoveAt(0);
        }
        
        public bool HasValidRefreshToken(string token)
        {
            return _refreshTokens.Exists(t => 
                t.Token == token && 
                t.ExpiryDate > DateTime.UtcNow && 
                !t.IsRevoked);
        }
        
        public void RevokeRefreshToken(string token)
        {
            var refreshToken = _refreshTokens.Find(t => t.Token == token);
            if (refreshToken != null)
            {
                refreshToken.Revoke();
                refreshToken.SetUpdatedBy(Id.ToString());
            }
        }
    }
}
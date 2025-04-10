using System;
using BiddingManagementSystem.Domain.Common;

namespace BiddingManagementSystem.Domain.Aggregates.UserAggregate
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public bool IsRevoked { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        
        private RefreshToken() { }
        
        public RefreshToken(string token, DateTime expiryDate)
        {
            Token = token;
            ExpiryDate = expiryDate;
            IsRevoked = false;
        }
        
        public void Revoke()
        {
            IsRevoked = true;
            RevokedAt = DateTime.UtcNow;
        }
    }
}
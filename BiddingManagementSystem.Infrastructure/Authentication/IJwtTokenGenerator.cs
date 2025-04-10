using System;
using BiddingManagementSystem.Domain.Aggregates.UserAggregate;

namespace BiddingManagementSystem.Infrastructure.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        DateTime GetRefreshTokenExpiryDate();
    }
}
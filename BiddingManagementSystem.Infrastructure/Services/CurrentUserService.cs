using System;
using System.Security.Claims;
using BiddingManagementSystem.Application.Contracts;
using BiddingManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace BiddingManagementSystem.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public string Username => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

        public UserRole? Role
        {
            get
            {
                var roleString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
                if (roleString != null && Enum.TryParse<UserRole>(roleString, out var role))
                {
                    return role;
                }
                return null;
            }
        }

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
} 
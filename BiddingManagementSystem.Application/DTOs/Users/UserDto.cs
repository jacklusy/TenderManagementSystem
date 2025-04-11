using System;
using BiddingManagementSystem.Application.DTOs.Common;
using BiddingManagementSystem.Domain.Enums;

namespace BiddingManagementSystem.Application.DTOs.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public string? CompanyName { get; set; }
        public string? RegistrationNumber { get; set; }
        public AddressDto? CompanyAddress { get; set; }
    }
}

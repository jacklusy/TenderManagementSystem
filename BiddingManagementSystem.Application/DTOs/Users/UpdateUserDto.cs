using System;
using BiddingManagementSystem.Application.DTOs.Common;

namespace BiddingManagementSystem.Application.DTOs.Users
{
    public class UpdateUserDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? CompanyName { get; set; }
        public string? RegistrationNumber { get; set; }
        public AddressDto? CompanyAddress { get; set; }
    }
}
using BiddingManagementSystem.Domain.Enums;

namespace BiddingManagementSystem.Application.DTOs.Auth
{
    public class RegisterUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public UserRole Role { get; set; }
        
        // Company details for bidders
        public string CompanyName { get; set; }
        public string RegistrationNumber { get; set; }
        public AddressDto CompanyAddress { get; set; }
    }
}
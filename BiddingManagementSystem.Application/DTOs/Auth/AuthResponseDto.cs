namespace BiddingManagementSystem.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }
        public string Role { get; set; }
    }
}
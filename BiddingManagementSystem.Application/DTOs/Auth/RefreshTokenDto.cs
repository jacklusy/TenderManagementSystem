namespace BiddingManagementSystem.Application.DTOs.Auth
{
    public class RefreshTokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
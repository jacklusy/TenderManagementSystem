using BiddingManagementSystem.Application.DTOs.Users;
using MediatR;

namespace BiddingManagementSystem.Application.Features.Users.Queries
{
    public class GetUserByUsernameQuery : IRequest<UserDto>
    {
        public string Username { get; set; }
    }
}
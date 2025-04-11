using BiddingManagementSystem.Application.DTOs.Auth;
using BiddingManagementSystem.Application.DTOs.Users;
using MediatR;

namespace BiddingManagementSystem.Application.Features.Users.Commands
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public RegisterUserDto RegisterUserDto { get; set; }
    }
}
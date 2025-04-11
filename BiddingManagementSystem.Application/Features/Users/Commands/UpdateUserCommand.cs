using BiddingManagementSystem.Application.DTOs.Users;
using MediatR;
using System;

namespace BiddingManagementSystem.Application.Features.Users.Commands
{
    public class UpdateUserCommand : IRequest<UserDto>
    {
        public Guid Id { get; set; }
        public UpdateUserDto UserDto { get; set; }
    }
}
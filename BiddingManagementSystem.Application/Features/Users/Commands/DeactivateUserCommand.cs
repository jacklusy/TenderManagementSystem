using BiddingManagementSystem.Application.DTOs.Users;
using MediatR;
using System;

namespace BiddingManagementSystem.Application.Features.Users.Commands
{
    public class DeactivateUserCommand : IRequest<UserDto>
    {
        public Guid Id { get; set; }
    }
}
using BiddingManagementSystem.Application.DTOs.Users;
using MediatR;
using System;

namespace BiddingManagementSystem.Application.Features.Users.Commands
{
    public class ActivateUserCommand : IRequest<UserDto>
    {
        public Guid Id { get; set; }
    }
}
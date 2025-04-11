using MediatR;
using System;

namespace BiddingManagementSystem.Application.Features.Users.Commands
{
    public class DeleteUserCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
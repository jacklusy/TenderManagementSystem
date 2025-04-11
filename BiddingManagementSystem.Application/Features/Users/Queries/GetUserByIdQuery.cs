using BiddingManagementSystem.Application.DTOs.Users;
using MediatR;
using System;

namespace BiddingManagementSystem.Application.Features.Users.Queries
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public Guid Id { get; set; }
    }
}
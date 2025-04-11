using BiddingManagementSystem.Application.DTOs.Users;
using MediatR;
using System.Collections.Generic;

namespace BiddingManagementSystem.Application.Features.Users.Queries
{
    public class GetAllUsersQuery : IRequest<List<UserDto>>
    {
    }
}
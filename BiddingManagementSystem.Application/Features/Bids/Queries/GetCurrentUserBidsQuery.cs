using BiddingManagementSystem.Application.DTOs.Bids;
using MediatR;
using System.Collections.Generic;

namespace BiddingManagementSystem.Application.Features.Bids.Queries
{
    public class GetCurrentUserBidsQuery : IRequest<List<BidDto>>
    {
    }
}
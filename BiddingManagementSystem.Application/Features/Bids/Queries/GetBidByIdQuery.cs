using BiddingManagementSystem.Application.DTOs.Bids;
using MediatR;
using System;

namespace BiddingManagementSystem.Application.Features.Bids.Queries
{
    public class GetBidByIdQuery : IRequest<BidDto>
    {
        public Guid Id { get; set; }
    }
}
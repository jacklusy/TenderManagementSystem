using BiddingManagementSystem.Application.DTOs.Bids;
using BiddingManagementSystem.Domain.Enums;
using MediatR;
using System.Collections.Generic;

namespace BiddingManagementSystem.Application.Features.Bids.Queries
{
    public class GetBidsByStatusQuery : IRequest<List<BidDto>>
    {
        public BidStatus Status { get; set; }
    }
}
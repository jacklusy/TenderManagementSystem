using BiddingManagementSystem.Application.DTOs.Bids;
using MediatR;
using System;
using System.Collections.Generic;

namespace BiddingManagementSystem.Application.Features.Bids.Queries
{
    public class GetBidsByTenderIdQuery : IRequest<List<BidDto>>
    {
        public Guid TenderId { get; set; }
    }
}
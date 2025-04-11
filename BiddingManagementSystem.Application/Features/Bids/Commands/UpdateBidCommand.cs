using BiddingManagementSystem.Application.DTOs.Bids;
using MediatR;
using System;

namespace BiddingManagementSystem.Application.Features.Bids.Commands
{
    public class UpdateBidCommand : IRequest<BidDto>
    {
        public Guid Id { get; set; }
        public UpdateBidDto BidDto { get; set; }
    }
}
using BiddingManagementSystem.Application.DTOs.Bids;
using MediatR;
using System;

namespace BiddingManagementSystem.Application.Features.Bids.Commands
{
    public class RejectBidCommand : IRequest<BidDto>
    {
        public Guid Id { get; set; }
        public string Reason { get; set; }
    }
}
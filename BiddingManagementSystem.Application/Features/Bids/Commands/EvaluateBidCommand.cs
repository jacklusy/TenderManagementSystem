using BiddingManagementSystem.Application.DTOs.Bids;
using MediatR;
using System;

namespace BiddingManagementSystem.Application.Features.Bids.Commands
{
    public class EvaluateBidCommand : IRequest<BidDto>
    {
        public Guid Id { get; set; }
        public decimal Score { get; set; }
        public string Comments { get; set; }
    }
}
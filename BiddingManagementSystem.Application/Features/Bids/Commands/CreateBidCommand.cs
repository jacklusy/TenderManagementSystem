using BiddingManagementSystem.Application.DTOs.Bids;
using MediatR;

namespace BiddingManagementSystem.Application.Features.Bids.Commands
{
    public class CreateBidCommand : IRequest<BidDto>
    {
        public CreateBidDto BidDto { get; set; }
    }
}


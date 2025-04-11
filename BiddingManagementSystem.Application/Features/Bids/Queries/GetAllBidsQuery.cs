using BiddingManagementSystem.Application.DTOs.Bids;
using MediatR;

namespace BiddingManagementSystem.Application.Features.Bids.Queries
{
    public class GetAllBidsQuery : IRequest<List<BidDto>>
    {
    }
}


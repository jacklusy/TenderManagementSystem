using MediatR;
using System;

namespace BiddingManagementSystem.Application.Features.Bids.Commands
{
    public class RemoveBidDocumentCommand : IRequest
    {
        public Guid BidId { get; set; }
        public Guid DocumentId { get; set; }
    }
}
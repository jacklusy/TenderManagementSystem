using BiddingManagementSystem.Application.DTOs.Bids;
using MediatR;
using System;

namespace BiddingManagementSystem.Application.Features.Bids.Commands
{
    public class AddBidDocumentCommand : IRequest<BidDocumentDto>
    {
        public Guid BidId { get; set; }
        public UploadBidDocumentDto DocumentDto { get; set; }
    }
}
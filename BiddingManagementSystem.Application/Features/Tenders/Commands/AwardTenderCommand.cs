using BiddingManagementSystem.Application.DTOs.Tenders;
using MediatR;
using System;

namespace BiddingManagementSystem.Application.Features.Tenders.Commands
{
    public class AwardTenderCommand : IRequest<TenderDto>
    {
        public Guid TenderId { get; set; }
        public Guid BidId { get; set; }
    }
} 
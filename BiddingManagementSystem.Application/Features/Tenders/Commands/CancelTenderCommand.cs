using BiddingManagementSystem.Application.DTOs.Tenders;
using MediatR;
using System;

namespace BiddingManagementSystem.Application.Features.Tenders.Commands
{
    public class CancelTenderCommand : IRequest<TenderDto>
    {
        public Guid Id { get; set; }
        public string Reason { get; set; }
    }
} 
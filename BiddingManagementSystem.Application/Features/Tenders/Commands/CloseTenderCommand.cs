using BiddingManagementSystem.Application.DTOs.Tenders;
using MediatR;
using System;

namespace BiddingManagementSystem.Application.Features.Tenders.Commands
{
    public class CloseTenderCommand : IRequest<TenderDto>
    {
        public Guid Id { get; set; }
    }
} 
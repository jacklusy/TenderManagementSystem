using BiddingManagementSystem.Application.DTOs.Tenders;
using MediatR;
using System;

namespace BiddingManagementSystem.Application.Features.Tenders.Commands
{
    public class UpdateTenderCommand : IRequest<TenderDto>
    {
        public Guid Id { get; set; }
        public UpdateTenderDto TenderDto { get; set; }
    }
}
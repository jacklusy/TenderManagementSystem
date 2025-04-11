using BiddingManagementSystem.Application.DTOs.Tenders;
using MediatR;
using System;

namespace BiddingManagementSystem.Application.Features.Tenders.Queries
{
    public class GetTenderByIdQuery : IRequest<TenderDto>
    {
        public Guid Id { get; set; }
    }
}
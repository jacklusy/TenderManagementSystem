using BiddingManagementSystem.Application.DTOs.Tenders;
using BiddingManagementSystem.Domain.Enums;
using MediatR;
using System.Collections.Generic;

namespace BiddingManagementSystem.Application.Features.Tenders.Queries
{
    public class GetTendersByStatusQuery : IRequest<List<TenderDto>>
    {
        public TenderStatus Status { get; set; }
    }
}
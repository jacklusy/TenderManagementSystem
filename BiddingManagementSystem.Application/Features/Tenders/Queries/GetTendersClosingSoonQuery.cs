using BiddingManagementSystem.Application.DTOs.Tenders;
using MediatR;
using System.Collections.Generic;

namespace BiddingManagementSystem.Application.Features.Tenders.Queries
{
    public class GetTendersClosingSoonQuery : IRequest<List<TenderDto>>
    {
        public int DaysThreshold { get; set; }
    }
} 
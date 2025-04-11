using BiddingManagementSystem.Application.DTOs.Tenders;
using MediatR;
using System.Collections.Generic;

namespace BiddingManagementSystem.Application.Features.Tenders.Queries
{
    public class GetTendersByCategoryQuery : IRequest<List<TenderDto>>
    {
        public string Category { get; set; }
    }
}
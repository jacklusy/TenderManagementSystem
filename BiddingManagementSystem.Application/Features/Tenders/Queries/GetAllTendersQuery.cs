using BiddingManagementSystem.Application.DTOs.Tenders;
using MediatR;
using System.Collections.Generic;

namespace BiddingManagementSystem.Application.Features.Tenders.Queries
{
    public class GetAllTendersQuery : IRequest<List<TenderDto>>
    {
    }
}
using BiddingManagementSystem.Application.DTOs.Tenders;
using MediatR;

namespace BiddingManagementSystem.Application.Features.Tenders.Commands
{
    public class CreateTenderCommand : IRequest<TenderDto>
    {
        public CreateTenderDto TenderDto { get; set; }
    }
}
